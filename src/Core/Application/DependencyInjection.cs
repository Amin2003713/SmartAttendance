using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Shifty.Application.Companies.Command.InitialCompany;
using Shifty.Domain.Tenants;
using System.Reflection;

namespace Shifty.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddCustomFluentValidation();
            ConfigureMaster();
            return services;
        }

        private static void AddCustomFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        }

        private static void ConfigureMaster()
        {
            TypeAdapterConfig<InitialCompanyCommand , ShiftyTenantInfo>.NewConfig().Map(dest => dest.Identifier , src => src.Domain);
        }
    }
}