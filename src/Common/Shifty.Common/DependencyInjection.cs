using Microsoft.Extensions.DependencyInjection;

namespace Shifty.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        return services;
    }
}