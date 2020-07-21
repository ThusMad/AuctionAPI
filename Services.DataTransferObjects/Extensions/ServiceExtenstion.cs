using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Services.DataTransferObjects.Extensions
{
    public static class ServiceExtenstion
    {
        public static void AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}