using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behavior
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);

            var timer = new Stopwatch();
            timer.Start();

            var response = await next();
            timer.Stop();
            var timeTaken = timer.Elapsed;
            if (timeTaken.Seconds > 3)
            {
                logger.LogWarning("Request {RequestName} took {TimeTaken} seconds", typeof(TRequest).Name, timeTaken.TotalSeconds);
            }
            else
            {
                logger.LogInformation("Request {RequestName} completed in {TimeTaken} seconds", typeof(TRequest).Name, timeTaken.TotalSeconds);
            }
            return response;
        }
    }
}
