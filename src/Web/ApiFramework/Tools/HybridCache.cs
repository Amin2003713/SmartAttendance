using System;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using SmartAttendance.Common.General;

namespace SmartAttendance.ApiFramework.Tools;

public static class HybridCache
{
    public static IServiceCollection AddHybridCaching(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = ApplicationConstant.AppOptions.RedisConnectionString;
        });

        services.AddHybridCache(options =>
        {
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration           = TimeSpan.FromHours(24),
                LocalCacheExpiration = TimeSpan.FromSeconds(10)
            };

            options.ReportTagMetrics = true;
        });

        return services;
    }
}