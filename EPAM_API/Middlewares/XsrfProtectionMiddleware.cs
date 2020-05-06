using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace EPAM_API.Middlewares
{
    public class XsrfProtectionMiddleware
    {
        private readonly IAntiforgery _antiforgery;
        private readonly RequestDelegate _next;

        public XsrfProtectionMiddleware(RequestDelegate next, IAntiforgery antiforgery)
        {
            _next = next;
            _antiforgery = antiforgery;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value.Contains("/uploads/"))
            {
                await _next.Invoke(context);
            }
            else
            {
                context.Response.Cookies.Append(
                    ".AspNetCore.Xsrf",
                    _antiforgery.GetAndStoreTokens(context).RequestToken,
                    new CookieOptions {HttpOnly = false, Secure = true, MaxAge = TimeSpan.FromMinutes(60)});

                await _next(context);
            }
        }
    }
}
