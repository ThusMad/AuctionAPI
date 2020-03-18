using System;
using System.Collections.Generic;
using System.Text;
using EPAM_BusinessLogicLayer.Interfaces;
using EPAM_BusinessLogicLayer.Services;
using EPAM_DataAccessLayer.Interfaces;
using EPAM_DataAccessLayer.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace EPAM_BusinessLogicLayer.Ninject
{
    public static class BllServiceCollectionExtensions
    {
        public static void AddBll(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUnitOfWork, EFUnitOfWork>();
        }
    }
}
