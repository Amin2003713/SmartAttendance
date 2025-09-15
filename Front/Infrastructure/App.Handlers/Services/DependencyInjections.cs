﻿using App.Persistence.Services;
using Microsoft.Extensions.DependencyInjection;

namespace App.Handlers.Services;

public static class DependencyInjections
{
    public static IServiceCollection AddHandlersServices(this IServiceCollection services)
    {
        services.AddInfrastructureServices();
        return services;
    }
}