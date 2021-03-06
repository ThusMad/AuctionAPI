﻿using System;
using System.IO;
using System.Text;
using EPAM_API.Extensions;
using EPAM_API.Helpers;
using EPAM_API.Services;
using EPAM_API.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Services.DatabaseExtensions;
using Services.DataTransferObjects.Extensions;

namespace EPAM_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddCors();
            services.AddControllers();

            services.AddAuctionConfiguration(appSettingsSection);
            services.AddAntiforgery(options => { options.HeaderName = "x-xsrf-token"; });

            services.AddContext(appSettings.ConnectionString);
            services.AddServices();
            services.AddMapper();

            services.AddAuctionAuthentication(key);
            services.AddAuctionAuthorization();

            services.AddHttpContextAccessor();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .WithOrigins("https://192.168.1.102:4200", "https://192.168.1.102", "http://192.168.1.102:4200", "http://192.168.1.102")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });

            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(10),
                ReceiveBufferSize = 4 * 1024
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseExceptionsHandler();
            app.UseTokenInterception();
            app.UseXsrfProtection();
            app.UseTimestampValidation();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            Directory.CreateDirectory("wwwroot/images");
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")),
                RequestPath = "/uploads"
            });
        }

    }
}
