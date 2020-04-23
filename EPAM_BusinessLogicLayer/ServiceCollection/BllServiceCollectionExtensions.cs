using System.Reflection;
using AutoMapper;
using EPAM_BusinessLogicLayer.BusinessModels.TokenStorage;
using EPAM_BusinessLogicLayer.BusinessModels.TokenStorage.Interfaces;
using EPAM_BusinessLogicLayer.Services;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using EPAM_DataAccessLayer.EF;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Interfaces;
using EPAM_DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EPAM_BusinessLogicLayer.ServiceCollection
{
    public static class BllServiceCollectionExtensions
    {
        public static void AddBll(this IServiceCollection services)
        {
            services.AddDbContext<AuctionContext>(opts =>
                opts.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Projects\EPAM_Auction\EPAM_DataAccessLayer\App_Data\Auction_DB.mdf;Integrated Security=True"));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuctionContext>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ITokenStorage, TokenStorage>();
            //services.AddTransient<IPaymentService, PaymentService>();
            //services.AddSingleton<IAuctionService, AuctionService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
