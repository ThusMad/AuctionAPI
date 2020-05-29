using System.Reflection;
using AutoMapper;
using EPAM_BusinessLogicLayer.Services;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using EPAM_DataAccessLayer.Contexts;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.UnitOfWork;
using EPAM_DataAccessLayer.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EPAM_BusinessLogicLayer
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBll(this IServiceCollection services)
        {
            services.AddDbContext<AuctionContext>(opts =>
                opts.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Projects\EPAM_Auction\EPAM_DataAccessLayer\App_Data\Auction_DB.mdf;Integrated Security=True"));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuctionContext>();

            
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddSingleton<IAuctionService, AuctionService>();
            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddSingleton<IUploadService, UploadService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
