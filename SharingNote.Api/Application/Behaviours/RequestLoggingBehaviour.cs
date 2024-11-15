namespace SharingNote.Api.Application.Behaviours
{
    public sealed class RequestLoggingBehaviour<TRequest, TResponse>(ILogger<RequestLoggingBehaviour<TRequest, TResponse>> _logger)
         : IPipelineBehavior<TRequest, TResponse>
         where TRequest : class
         where TResponse : Ardalis.Result.IResult
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            string moduleName = GetModuleName(typeof(TRequest).FullName!);
            string requestName = typeof(TRequest).Name;

            using (LogContext.PushProperty("Module", moduleName))
            {
                _logger.LogInformation("Handling {RequestName} with content: {@Request}.", requestName, request);

                var stopwatch = Stopwatch.StartNew();

                var response = await next();

                _logger.LogInformation("Request {RequestName} processed in {ElapsedMilliseconds} ms.", requestName, stopwatch.ElapsedMilliseconds);

                stopwatch.Stop();

                _logger.LogInformation("Response status of the {RequestName}: {ResponseStatusCode}", requestName, response.Status);

                return response;
            }
        }

        private string GetModuleName(string requestName) => requestName.Split('.')[2];
    }
}
