namespace SharingNote.Api.Application.Behaviours
{
    public sealed class ExceptionHandlingBehaviour<TRequest, TResponse>(ILogger<ExceptionHandlingBehaviour<TRequest, TResponse>> logger)
         : IPipelineBehavior<TRequest, TResponse>
         where TRequest : class
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                var response = await next();
                return response;
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                logger.LogError(ex, "Unhandled exception for {RequestName}", requestName);
                throw new Exception(requestName, innerException: ex);
            }
        }
    }
}
