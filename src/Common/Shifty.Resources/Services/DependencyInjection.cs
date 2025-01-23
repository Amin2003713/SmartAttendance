using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shifty.Resources.ExceptionMessages.Common;
using System.Globalization;
using System.Reflection;

namespace Shifty.Resources.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddResources(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            
            services.Configure<RequestLocalizationOptions>(options =>
                                                                   {
                                                                       IList<CultureInfo> supportedCultures = new[]
                                                                       {
                                                                           new CultureInfo("fa-IR") , new CultureInfo("en-US") ,
                                                                       };
                                                                       options.DefaultRequestCulture = new RequestCulture("en-US");
                                                                       options.SupportedCultures     = supportedCultures;
                                                                       options.SupportedUICultures   = supportedCultures;
                                                                       options.RequestCultureProviders.Insert(0 ,
                                                                           new CustomRequestCultureProvider(context =>
                                                                                                            {
                                                                                                                var culture = context.Request.Headers["Accept-Language"].
                                                                                                                    ToString().
                                                                                                                    Split(',').
                                                                                                                    FirstOrDefault();
                                                                                                                return Task.FromResult(
                                                                                                                    new ProviderCultureResult(culture));
                                                                                                            }));
                                                                   });


            var assembly       = (typeof(BaseLocalizer<>)).Assembly;
            var exceptionTypes = assembly.GetTypes().Where(type => type.Name.EndsWith("Messages") && type is { IsClass: true , IsAbstract: false });

            foreach (var exceptionType in exceptionTypes)
                services.AddSingleton(exceptionType);


            return services;
        }

        public static void UseResources(this IApplicationBuilder app)
        {
            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>()?.Value;
            if (localizationOptions != null)
                app.UseRequestLocalization(localizationOptions);
        }

    }
}