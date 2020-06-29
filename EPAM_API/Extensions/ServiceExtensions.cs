using EPAM_API.Helpers;
using EPAM_API.Services;
using EPAM_API.Services.Interfaces;
using EPAM_DataAccessLayer.UnitOfWork;
using EPAM_DataAccessLayer.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Services.AccountService.Interfaces;
using Services.AccountService.Service;
using Services.AuctionService.Interfaces;
using Services.AuctionService.Service;
using Services.BalanceService.Interfaces;
using Services.BalanceService.Service;
using Services.CategoryService.Interfaces;
using Services.CategoryService.Service;
using Services.TokenService.Interfaces;
using Services.TokenService.Service;
using Services.UploadService.Interfaces;
using Services.UploadService.Service;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Services.PaymentService.Interfaces;
using Services.PaymentService.Service;

namespace EPAM_API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAuctionService, AuctionService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddSingleton<IUploadService, UploadService>();
            services.AddTransient<IBalanceService, BalanceService>();
            services.AddTransient<IPaymentService, PaymentService>();

            services.AddTransient<ITokenProvider, TokenProvider>();
            services.AddTransient<IUserProvider, UserProvider>();
        }

        public static void AddAuctionAuthentication(this IServiceCollection services, byte[] securityKey)
        {
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        public static void AddAuctionAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(auth =>
            {
                auth.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireClaim(ClaimTypes.Role)
                    .Build();
            });
        }

        public static void AddAuctionConfiguration(this IServiceCollection services, IConfiguration settings)
        {
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.Configure<AppSettings>(settings);
        }

    }
}
