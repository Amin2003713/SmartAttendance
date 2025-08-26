using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Shifty.ApiFramework;
using Shifty.ApiFramework.Analytics;
using Shifty.ApiFramework.Configuration;
using Shifty.ApiFramework.Injections;
using Shifty.ApiFramework.Middleware.Jwt;
using Shifty.Application;
using Shifty.Application.Base.Payment.Commands.CreatePayment;
using Shifty.Common;
using Shifty.Common.General;
using Shifty.Domain.Tenants;
using Shifty.Domain.Users;
using Shifty.Persistence.Db;
using Shifty.Persistence.Services;
using Shifty.RequestHandlers.Services;

namespace Shifty.Api.Services;

/// <summary>
///     Entry point for application service configuration.
/// </summary>
public class Startup
{
    /// <summary>
    ///     Configure built-in and custom services via Microsoft DI.
    /// </summary>
    public void ConfigureServices(IServiceCollection services)
    {
        // Add Serilog for structured logging
        services.AddSerilogLogging();

        // Register core WebAPI setup using a shared generic setup method
        services.AddWebApi<
            User,                  // User entity model
            Role,                  // RoleTypes entity model
            ShiftyTenantInfo,      // Tenant info model
            ShiftyTenantDbContext, // Multi-tenant DB context
            ShiftyDbContext,       // Service-specific DB context (non-tenant)
            Program,               // Reference for localizer (error messages)
            CreatePaymentCommand   // Optional: Commands for custom Swagger sample
        >(
            AddLoginRecordForUsers // Delegate to log login success for auditing
        );

        // Register internal layers (Application, Persistence, etc.)
        services.AddApiFramework();
        services.AddApplication();
        services.AddPersistence();
        services.AddHandler();
        services.AddCommon();
    }

    private static Task AddLoginRecordForUsers(TokenValidatedContext context)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Register custom dependencies via Autofac container builder.
    /// </summary>
    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterServices(); // Defined in AutofacConfigurationExtensions
    }

    /// <summary>
    ///     Configure middleware and HTTP request pipeline.
    /// </summary>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Show developer exception page only in dev/staging
        if (env.IsDevelopment() || env.IsStaging())
            app.UseDeveloperExceptionPage();

        // Use centralized JWT exception handler
        app.UseMiddleware<JwtExceptionHandlingMiddleware>();

        // Use Serilog request logging
        app.UseSerilogRequestLogging();

        // Register shared WebAPI conventions including Swagger and resource tracking
        app.UseWebApi<Empty>("Tenant API Reference");
    }
}