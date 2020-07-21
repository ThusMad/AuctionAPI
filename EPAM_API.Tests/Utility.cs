using System.Collections.Generic;
using AutoMapper;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using Services.AccountService.Interfaces;
using Services.AccountService.Service;
using Services.DataTransferObjects.Profiles;
using Services.TokenService.Interfaces;
using Services.TokenService.Service;

namespace EPAM_API.Tests
{
    public static class Utility
    {
        public static Mock<IUnitOfWork> MockUnitOfWork()
        {
            return new Mock<IUnitOfWork>();
        }

        public static IMapper GetMapper()
        {
            return new MapperConfiguration(opts => { opts.AddProfile<CategoryProfile>(); }).CreateMapper();
        }


        public static IAccountService GetAccountService(IMapper mapper, IUnitOfWork uow, UserManager<ApplicationUser> userManager)
        {
            return new AccountService(uow, userManager, mapper);
        }

        public static ITokenService GetTokenService(IUnitOfWork uow, UserManager<ApplicationUser> userManager)
        {
            return new TokenService(uow, userManager);
        }
    }
}