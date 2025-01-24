using System.Net;

namespace ventry_api.Shared.Exceptions
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (type, statusCode, message) = exception switch
            {
                FluentValidation.ValidationException validationEx =>
                (
                    "VALIDATION_ERROR",
                    (int)HttpStatusCode.BadRequest,
                    string.Join(", ", validationEx.Errors.Select(e => e.ErrorMessage))
                ),
                ApiException ex => (ex.ErrorCode, ex.StatusCode, ex.Message),
                KeyNotFoundException => ("NOT_FOUND", (int)HttpStatusCode.NotFound, exception.Message),
                UnauthorizedAccessException => ("UNAUTHORIZED", (int)HttpStatusCode.Unauthorized, exception.Message),
                _ => ("INTERNAL_ERROR", (int)HttpStatusCode.InternalServerError, "An unexpected error occurred")
            };

            // Log based on severity
            if (statusCode >= 500)
                _logger.LogError(exception, "An error occurred: {Message}", message);
            else
                _logger.LogWarning(exception, "An error occurred: {Message}", message);

            // Prepare and send error response
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse(type, statusCode, message);
            await context.Response.WriteAsJsonAsync(response);
        }

    }

    // Extension method for easy middleware registration
    public static class ExceptionHandlingExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
