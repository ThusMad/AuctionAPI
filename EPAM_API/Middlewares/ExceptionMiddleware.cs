using System;
using System.Net;
using System.Threading.Tasks;
using EPAM_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Infrastructure.Exceptions;

namespace EPAM_API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<TimeValidationMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex).ConfigureAwait(false);
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

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = (int)HttpStatusCode.InternalServerError;

            if (ex is ErrorException errorException) code = errorException.ErrorCode;
            else if (ex is RefreshTokenExpireException) code = (int)HttpStatusCode.Unauthorized;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;
            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = code,
                Message = ex.Message
            }.ToString());
        }
    }
}
