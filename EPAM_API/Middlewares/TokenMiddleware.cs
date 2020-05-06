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

                if (!context.Response.Headers.ContainsKey("X-Content-Type-Options"))
                {
                    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                }
                else
                {
                    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                }

                if (!context.Response.Headers.ContainsKey("X-Xss-Protection"))
                {
                    context.Response.Headers.Add("X-Xss-Protection", "1");
                }
                else
                {
                    context.Response.Headers["X-Xss-Protection"] = "1";
                }

                if (!context.Response.Headers.ContainsKey("X-Frame-Options"))
                {
                    context.Response.Headers.Add("X-Frame-Options", "DENY");
                }
                else
                {
                    context.Response.Headers["X-Frame-Options"] = "DENY";
                }
                
                await _next.Invoke(context);
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
