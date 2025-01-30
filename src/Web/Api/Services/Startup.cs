using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Shifty.ApiFramework.Configuration;
using Shifty.Application;
using Shifty.Common;
using Shifty.Common.General;
using Shifty.Persistence.Services;
using Shifty.RequestHandlers.Services;
using Shifty.Resources.Services;

namespace Shifty.Api.Services
{
    public class Startup(IConfiguration configuration)
    {

        private IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResources();
            services.AddWebApi(Configuration);
            services.AddApplication();
            services.AddPersistence(Configuration);
            services.AddHandler();
            services.AddCommon(Configuration);
        }

        //Register Services to Autofac ContainerBuilder
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterServices();
        }

        public void Configure(IApplicationBuilder app , IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();
            app.UseResources();
            app.UseWebApi(env);
        }
    }
}