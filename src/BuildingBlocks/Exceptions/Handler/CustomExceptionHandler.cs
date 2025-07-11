using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Exceptions.Handler
{
    public class CustomExceptionHandler (ILogger<CustomExceptionHandler> logger)
        : IExceptionHandler
    {

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

            (string Detail, string Title, int StatusCode) problemDetails = exception switch
            {
                InternalServerException => (
                    Detail: exception.Message,
                    Title: exception.GetType().Name,
                    StatusCode: StatusCodes.Status500InternalServerError
                ),
                ValidationException => (
                    Detail: exception.Message,
                    Title: exception.GetType().Name,
                    StatusCode: StatusCodes.Status400BadRequest
                ),
                BadRequestException => (
                    Detail: exception.Message,
                    Title: exception.GetType().Name,
                    StatusCode: StatusCodes.Status400BadRequest
                ),
                NotFoundException => (
                    Detail: exception.Message,
                    Title: exception.GetType().Name,
                    StatusCode: StatusCodes.Status404NotFound
                ),
                _ => (
                    Detail: "An unexpected error occurred.",
                    Title: "Internal Server Error",
                    StatusCode: StatusCodes.Status500InternalServerError
                )
            };
            var problemDetail = new ProblemDetails
            {
                Title = problemDetails.Title,
                Detail = problemDetails.Detail,
                Status = problemDetails.StatusCode,
                Instance = httpContext.Request.Path
            };
            problemDetail.Extensions.Add("traceId", httpContext.TraceIdentifier);
            if (exception is ValidationException validationException)
            {
                problemDetail.Extensions.Add("errors", validationException.ValidationResult?.ErrorMessage);
            }
            await httpContext.Response.WriteAsJsonAsync(problemDetail, cancellationToken: cancellationToken);
            return true;
        }
    }
}
