using SharingNote.Api.Application.Messaging;

namespace SharingNote.Api.Application.Behaviours
{
    public sealed class ValidationBehaviour<TRequest, TResponse>(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehaviour<TRequest, TResponse>> _logger
    ) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommandBase
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Validating {RequestName}", typeof(TRequest).Name);

            if (!validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                validators.Select(x => x.ValidateAsync(context, cancellationToken)));

            var validationFailures = validationResults
                .Where(x => !x.IsValid)
                .SelectMany(x => x.Errors)
                .ToArray();

            if (!validationFailures.Any())
            {
                _logger.LogInformation("{RequestName} is valid.", typeof(TRequest).Name);
                return await next();
            }

            _logger.LogInformation("{RequestName} has validation errors: {@ValidationErrors}.", typeof(TRequest).Name, validationFailures);

            if (typeof(TResponse).IsGenericType
                && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultType = typeof(TResponse).GetGenericArguments()[0];
                var invalidResult = typeof(Result<>)
                    .MakeGenericType(resultType)
                    .GetMethod("Invalid", [typeof(ValidationError[])])
                    !.Invoke(null, [CreateValidationErrors(validationFailures)]);
                return (TResponse)invalidResult!;
            }
            else if (typeof(TResponse) == typeof(Result))
            {
                var result = Result.Invalid(CreateValidationErrors(validationFailures));
                return (TResponse)(object)result;
            }

            throw new ValidationException(validationFailures);

        }

        static ValidationError[] CreateValidationErrors(ValidationFailure[] validationFailures)
        {
            return validationFailures
                .Select(x =>
                {
                    var severity = x.Severity switch
                    {
                        Severity.Error => ValidationSeverity.Error,
                        Severity.Warning => ValidationSeverity.Warning,
                        Severity.Info => ValidationSeverity.Info,
                        _ => ValidationSeverity.Error
                    };

                    return new ValidationError(
                        x.PropertyName,
                        x.ErrorMessage,
                        x.ErrorCode,
                        severity
                    );
                })
                .ToArray();
        }
    }
}
