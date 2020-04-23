using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using EPAM_BusinessLogicLayer.BusinessModels;
using EPAM_BusinessLogicLayer.Services;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using EPAM_DataAccessLayer.EF;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Interfaces;
using EPAM_DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EPAM_BusinessLogicLayer.ServiceCollection
{
    public static class BllServiceRoleSeedExtension
    {
        public static void SeedRoles(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AuctionContext>();

                string[] roles = { Roles.Owner, Roles.Administrator, Roles.Moderator, Roles.User, Roles.Premium, Roles.Plus };
                var roleStore = new RoleStore<IdentityRole>(context);

                foreach (var role in roles)
                {
                    if (!context.Roles.Any(r => r.Name == role))
                    {
                        roleStore.CreateAsync(new IdentityRole(role));
                    }
                }
            }

            
        }
    }
}
