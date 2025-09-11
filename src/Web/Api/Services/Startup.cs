using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SmartAttendance.ApiFramework;
using SmartAttendance.ApiFramework.Configuration;
using SmartAttendance.ApiFramework.Injections;
using SmartAttendance.ApiFramework.Middleware.Jwt;
using SmartAttendance.Application;
using SmartAttendance.Application.Features.UserPasswords.Commands.Create;
using SmartAttendance.Common;
using SmartAttendance.Common.General;
using SmartAttendance.Domain.Tenants;
using SmartAttendance.Domain.Users;
using SmartAttendance.Persistence.Db;
using SmartAttendance.Persistence.Services;
using SmartAttendance.RequestHandlers.Services;

namespace SmartAttendance.Api.Services;

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
        

        // Register core WebAPI setup using a shared generic setup method
        services.AddWebApi<
            User,                           // User entity model
            Role,                           // RoleTypes entity model
            SmartAttendanceTenantInfo,      // Tenant info model
            SmartAttendanceTenantDbContext, // Multi-tenant DB context
            SmartAttendanceDbContext,       // Service-specific DB context (non-tenant)
            Program,                        // Reference for localizer (error messages)
            CreateUserPasswordCommand            // Optional: Commands for custom Swagger sample
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