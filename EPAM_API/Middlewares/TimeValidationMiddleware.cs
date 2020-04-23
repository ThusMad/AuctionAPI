using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                _logger.LogInformation($"Processing request {context.Request.Method} {context.Request.Path}");

                var timestamp = context.Request.Query["timestamp"];
                var recvWindow = context.Request.Query["recvWindow"];

                if (timestamp == string.Empty)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync($"Malformed request, parameter {nameof(timestamp)} must be set");
                }
                var timestampVal = long.Parse(timestamp);
                int? recvWindowVal = null;
                if (recvWindow != string.Empty)
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
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(e.Message);
            }
            finally
            {
                _logger.LogInformation(
                    "Request {method} {url} => {statusCode}",
                    context.Request?.Method,
                    context.Request?.Path.Value,
                    context.Response?.StatusCode);
            }
        }
    }
}
