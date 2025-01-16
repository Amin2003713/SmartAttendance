using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Shifty.Common.General;
using Shifty.Domain.Constants;
using System;
using System.Collections.Generic;

namespace Shifty.ApiFramework.Aspire;

public static class AspireExtensions
{
     public static IServiceCollection AddServiceDefaults(this IServiceCollection services)
    {
        services.Configure<OtlpExporterOptions>(options =>
        {
            options.Headers = $"x-otlp-api-key=FC83FFEF-1C71-4C88-97D7-27CE9570F131";
        });

        services.AddOpenTelemetry()
            .ConfigureResource(resourceBuilder =>
            {
                resourceBuilder.AddService(
                    serviceName: ApplicationConstant.ApplicationName,
                    serviceVersion: "1.0.0",
                    autoGenerateServiceInstanceId: true);

                resourceBuilder.AddAttributes(new Dictionary<string, object>
                {
                    ["deployment.environment"] =
#if DEBUG
                        "Development"
#else
                        "Production"
#endif
                    ,
                    ["region"] = "us-east-1"
                });
            })
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation()
                       .AddRuntimeInstrumentation();

                metrics.AddMeter(ApplicationConstant.ApplicationName);

                metrics.AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(ApplicationConstant.OpenTelemetryEndpoint);
                });
            })
            .WithTracing(tracing =>
            {
                tracing.AddSource(ApplicationConstant.ApplicationName)
                       .AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation();

                tracing.SetSampler(new ParentBasedSampler(new TraceIdRatioBasedSampler(0.5)));

                tracing.AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(ApplicationConstant.OpenTelemetryEndpoint);
                });
            });

        services.AddLogging(builder =>
        {
            builder.AddOpenTelemetry(options =>
            {
                options.IncludeFormattedMessage = true;
                options.IncludeScopes = true;
                options.ParseStateValues = true;

                options.AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri(ApplicationConstant.OpenTelemetryEndpoint);
                });
            });
        });

        return services;
    }

    private static TBuilder AddOpenTelemetryExporters<TBuilder>(
        this TBuilder builder
        , IConfiguration configuration)
        where TBuilder : IHostApplicationBuilder
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        // Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.AspNetCore package)
        //if (!string.IsNullOrEmpty(configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
        //{
        //    builder.Services.AddOpenTelemetry()
        //       .UseAzureMonitor();
        //}

        return builder;
    }

    public static IServiceCollection AddDefaultHealthChecks(this IServiceCollection services , AppOptions options)
    {
        services.AddHealthChecks().
                 AddCheck("sqlServerHealth"
                     , () =>
                       {
                           try
                           {
                               using var connection = new SqlConnection(options.TenantStore);
                               connection.Open();
                               return HealthCheckResult.Healthy();
                           }
                           catch (Exception ex)
                           {
                               return HealthCheckResult.Unhealthy(ex.Message);
                           }
                       }
                     , new[]
                     {
                         "live"
                     }).
                 AddCheck("self"
                     , () => HealthCheckResult.Healthy()
                     , new[]
                     {
                         "live"
                     });

        return services;
    }

    public static IApplicationBuilder UseDefaultEndpoints(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health");
        app.UseHealthChecks("/alive"
            , new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live")
            });

        return app;
    }
}