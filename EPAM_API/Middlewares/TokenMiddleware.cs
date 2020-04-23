using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPAM_API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EPAM_API.Middlewares
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public TokenMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this._next = next;
            _logger = loggerFactory.CreateLogger<TokenMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                _logger.LogInformation($"Processing request {context.Request.Method} {context.Request.Path}");

                var token = context.Request.Cookies[".AspNetCore.Application.Id"];
                var refreshToken = context.Request.Cookies[".AspNetCore.Application.Cre"];
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    context.Request.Headers.Add("RefreshToken", refreshToken);
                }
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                }
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Xss-Protection", "1");
                context.Response.Headers.Add("X-Frame-Options", "DENY");

                await _next.Invoke(context);
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
