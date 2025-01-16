using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Data.SqlClient;
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
    public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.Configure<OtlpExporterOptions>(options =>
                                                        {
                                                            options.Headers = $"x-otlp-api-key=FC83FFEF-1C71-4C88-97D7-27CE9570F131";
                                                        });

       builder.Services.AddOpenTelemetry()
    .ConfigureResource(resourceBuilder =>
    {
        // Add service name and version to the resource
        resourceBuilder.AddService(
            serviceName: builder.Environment.ApplicationName,
            serviceVersion: "1.0.0",
            autoGenerateServiceInstanceId: true);


        // Add additional attributes (e.g., environment, deployment region)
        resourceBuilder.AddAttributes(new Dictionary<string, object>
        {
            ["deployment.environment"] = builder.Environment.EnvironmentName,
            ["region"] = "us-east-1"
        });
    })
    .WithMetrics(metrics =>
    {
        // Add default instrumentation
        metrics.AddAspNetCoreInstrumentation()
               .AddHttpClientInstrumentation()
               .AddRuntimeInstrumentation();

        // Add custom metrics
        metrics.AddMeter(ApplicationConstant.ApplicationName);

        builder.Services.AddRequestTimeouts(
            configure: static timeouts =>
                           timeouts.AddPolicy("HealthChecks" , TimeSpan.FromSeconds(5)));

        builder.Services.AddOutputCache(
            configureOptions: static caching =>
                                  caching.AddPolicy("HealthChecks" , build: static policy => policy.Expire(TimeSpan.FromSeconds(10))));
        // Export metrics to Prometheus or an OpenTelemetry Collector
        metrics.AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(ApplicationConstant.OpenTelemetryEndpoint); // OpenTelemetry Collector endpoint
        });
    })
    .WithTracing(tracing =>
    {
        // Add default instrumentation
        tracing.AddSource(builder.Environment.ApplicationName)
               .AddAspNetCoreInstrumentation()
               .AddHttpClientInstrumentation();

        // Configure sampling
        tracing.SetSampler(new ParentBasedSampler(new TraceIdRatioBasedSampler(0.5)));

        // Export traces to Jaeger or an OpenTelemetry Collector
        tracing.AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri("http://otel-collector:4317"); // OpenTelemetry Collector endpoint
        });
    });

// Add OpenTelemetry logging
builder.Logging.AddOpenTelemetry(options =>
{
    options.IncludeFormattedMessage = true;
    options.IncludeScopes = true;
    options.ParseStateValues = true;

    // Export logs to an OpenTelemetry Collector
    options.AddOtlpExporter(otlpOptions =>
    {
        otlpOptions.Endpoint = new Uri("http://otel-collector:4317");
    });
});

        return builder;
    }

    private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        // Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.AspNetCore package)
        //if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
        //{
        //    builder.Services.AddOpenTelemetry()
        //       .UseAzureMonitor();
        //}

        return builder;
    }

    public static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder , AppOptions options) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddHealthChecks().
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
                    })
               // Add a default liveness check to ensure app is responsive
               .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        app.MapGroup("/health").CacheOutput("HealthChecks").WithRequestTimeout("HealthChecks");
        app.MapHealthChecks("/health");
        app.MapHealthChecks("/alive"
            , new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live") ,
            });
        return app;
    }
}