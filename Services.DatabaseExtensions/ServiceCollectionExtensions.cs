using System.Reflection;
using EPAM_DataAccessLayer.Contexts;
using EPAM_DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Services.DatabaseExtensions
{
    public static class DatabaseExtension
    {
        public static void AddContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AuctionContext>(opts =>
                opts.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuctionContext>();
        }
    }
}
