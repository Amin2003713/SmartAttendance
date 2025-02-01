using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Shifty.Common.Behaviours;
using Shifty.Resources.Common;

namespace Shifty.RequestHandlers.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHandler(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                                {
                                    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                                    cfg.AddOpenBehavior(typeof(RequestResponseLoggingBehavior<,>));
                                    

                                });

            return services;
        }



    }
}