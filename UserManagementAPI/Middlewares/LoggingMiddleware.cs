using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace UserManagementAPI.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log the HTTP method and request path
            _logger.LogInformation("HTTP Request: {Method} {Path}", context.Request.Method, context.Request.Path);

            // Proceed to the next middleware
            await _next(context);

            // Log the response status code
            _logger.LogInformation("HTTP Response: {StatusCode}", context.Response.StatusCode);
        }
    }
}
