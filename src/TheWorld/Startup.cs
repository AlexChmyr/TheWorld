using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.EntityFrameworkCore;
using TheWorld.Models;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using TheWorld.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace TheWorld
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(hostingEnvironment.ContentRootPath);
            builder.AddJsonFile("config.json");
            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(opt => 
                {
#if !DEBUG
                    opt.Filters.Add(new RequireHttpsAttribute());
#endif
                })
                .AddJsonOptions(opt => 
                {
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services.AddIdentity<WorldUser, IdentityRole>(conf => 
            {
                conf.User.RequireUniqueEmail = true;
                conf.Password.RequiredLength = 6;
                conf.Password.RequireNonAlphanumeric = false;
                conf.Password.RequireLowercase = false;
                conf.Password.RequireUppercase = false;
                conf.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
            })
            .AddEntityFrameworkStores<WorldContext>();

            services.AddLogging();

            services.AddDbContext<WorldContext>();
            //var connection = @"Server=(localdb)\mssqllocaldb;Database=EFGetStarted.AspNetCore.NewDb;Trusted_Connection=True;";
            //services.AddDbContext<WorldContext>(options => options.UseSqlServer(connection));

            services.AddScoped<IMailService, DebugMailService>();
            services.AddScoped<IWorldRepository, WorldRepository>();
            services.AddTransient<WorldContextSeedData>();
            services.AddScoped<GeoLocService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, WorldContextSeedData seeder)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug(LogLevel.Warning);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvc(config => 
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "App", action = "Index"}
                    );
            });

            Mapper.Initialize(config =>
            {
                config.CreateMap<Trip, TripViewModel>().ReverseMap();
                config.CreateMap<Stop, StopViewModel>().ReverseMap();
            });

            await seeder.EnsureSeedDataAsync();
        }
    }
}
