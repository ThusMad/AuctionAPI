using Microsoft.AspNetCore.Builder;

namespace EPAM_API.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseXsrfProtection(this IApplicationBuilder builder)
            => builder.UseMiddleware<XsrfProtectionMiddleware>();
        public static IApplicationBuilder UseTimestampValidation(this IApplicationBuilder builder)
            => builder.UseMiddleware<TimeValidationMiddleware>();
        public static IApplicationBuilder UseTokenInterception(this IApplicationBuilder builder)
            => builder.UseMiddleware<TokenMiddleware>();
        public static IApplicationBuilder UseExceptionsHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<ExceptionMiddleware>();
    }
}
