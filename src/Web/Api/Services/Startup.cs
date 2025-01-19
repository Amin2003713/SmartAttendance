using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shifty.ApiFramework.Configuration;
using Shifty.Application;
using Shifty.Common;
using Shifty.Common.General;
using Shifty.Persistence.Services;

namespace Shifty.Api.Services
{
    public class Startup
    {
        private readonly SiteSettings _siteSetting;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _siteSetting  = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddPersistence(Configuration);
            services.AddCommon(Configuration);
            services.AddWebApi(Configuration , _siteSetting);
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

            app.UseWebApi(Configuration , env);
        }
    }
}