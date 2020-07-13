using System.Threading.Tasks;
using EPAM_API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EPAM_API.Middlewares
{
    public class TimeValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public TimeValidationMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this._next = next;
            _logger = loggerFactory.CreateLogger<TimeValidationMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value.Contains("/uploads/"))
            {
                await _next.Invoke(context);
            }
            else
            {
                var timestamp = context.Request.Query["timestamp"];
                var recvWindow = context.Request.Query["recvWindow"];
                int? recvWindowVal = null;
                long? timestampVal = null;

                if (string.IsNullOrEmpty(timestamp))
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync($"Malformed request, parameter {nameof(timestamp)} must be set");
                }
                else
                {
                    timestampVal = long.Parse(timestamp);
                }

                if (!string.IsNullOrEmpty(recvWindow))
                {
                    recvWindowVal = int.Parse(recvWindow);
                }
                if (!TimestampValidator.Validate(timestampVal, recvWindowVal))
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync($"Timestamp for this request is outside of the recvWindow");
                }
                else
                {
                    await _next.Invoke(context);
                }
            }
        }
    }
}
