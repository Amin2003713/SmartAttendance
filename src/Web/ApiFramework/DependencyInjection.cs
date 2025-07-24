using Microsoft.Extensions.DependencyInjection;

namespace Shifty.ApiFramework;

public static class DependencyInjection
{
    public static IServiceCollection AddApiFramework(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        return services;
    }
}