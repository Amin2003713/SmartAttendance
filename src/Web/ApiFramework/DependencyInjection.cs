using Microsoft.Extensions.DependencyInjection;

namespace SmartAttendance.ApiFramework;

public static class DependencyInjection
{
    public static IServiceCollection AddApiFramework(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        return services;
    }
}