using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shifty.Resources.Common;
using System.Globalization;
using System.Reflection;

namespace Shifty.Resources.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddResources(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Remove this part
            var assembly       = typeof(BaseLocalizer<>).Assembly;
            var exceptionTypes = assembly.GetTypes().
                                           Where(type => type.Name.EndsWith("Messages") && type is { IsClass: true , IsAbstract: false } &&
                                                         type.Namespace?.Contains("Shifty.Resources.Resources.Messages") != true);
            foreach (var exceptionType in exceptionTypes)
                services.AddSingleton(exceptionType);

            services.AddSingleton(typeof(BaseLocalizer<>));
            return services;
        }

        public static void UseResources(this IApplicationBuilder app)
        {
            app.UseRequestLocalization(options =>
                                       {
                                           options.RequestCultureProviders.Clear();

                                           IList<CultureInfo> supportedCultures = new[]
                                           {
                                               new CultureInfo("fa-IR") , new CultureInfo("en-US") ,
                                           };
                                           options.SupportedCultures     = supportedCultures;
                                           options.SupportedUICultures   = supportedCultures;
                                           options.DefaultRequestCulture = new RequestCulture("fa-IR");
                                           options.RequestCultureProviders.Insert(0 ,
                                               new CustomRequestCultureProvider(context =>
                                                                                {
                                                                                    // var culture = context.Request.Headers["Accept-Language"].
                                                                                    //                       ToString().
                                                                                    //                       Split(',').
                                                                                    //                       FirstOrDefault();
                                                                                    // if (!string.IsNullOrEmpty(culture) &&
                                                                                    //     supportedCultures.Any(
                                                                                    //         c => c.Name.Equals(culture , StringComparison.OrdinalIgnoreCase)) && culture != "*")
                                                                                    //     return Task.FromResult(new ProviderCultureResult(culture));

                                                                                    return Task.FromResult(new ProviderCultureResult("fa-IR")); // Default culture


                                                                                }));
                                       });
        }  

    }
}