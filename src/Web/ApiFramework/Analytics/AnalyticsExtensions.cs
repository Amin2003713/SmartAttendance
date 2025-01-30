using System;
using System.Collections.Generic;
using Grafana.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;
using Shifty.Common.General;

namespace Shifty.ApiFramework.Analytics
{
    public static class AnalyticsExtensions
    {
        public static IServiceCollection AddOpenTelemetryServices(this IServiceCollection services)
        {
            services.AddOpenTelemetry().
                     WithMetrics(builder =>
                                 {
                                     builder.UseGrafana()
                                            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ApplicationConstant.ApplicationName)).
                                            AddAspNetCoreInstrumentation().
                                            AddHttpClientInstrumentation().
                                            AddRuntimeInstrumentation().
                                            AddProcessInstrumentation().
                                            AddPrometheusExporter(); // ✅ Prometheus for metrics
                                 }).
                     WithTracing(builder =>
                                 {
                                     builder.UseGrafana()
                                            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ApplicationConstant.ApplicationName)).
                                            AddAspNetCoreInstrumentation().
                                            AddHttpClientInstrumentation().
                                            AddOtlpExporter(options =>
                                                            {
                                                                options.Endpoint = new Uri("http://otel-collector:4317"); // ✅ OTLP Tracing
                                                            });
                                 });

            services.AddLogging(builder => builder.AddOpenTelemetry(options =>
                                                             {
                                                                 options.UseGrafana()
                                                                        .AddConsoleExporter()
                                                                        .AddOtlpExporter(ApplicationConstant.Aspire.OtlpExporter);
                                                             }));
            return services;
        }

        public static IHostBuilder UseSerilogLogging(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseSerilog((hostBuilderContext , loggerConfiguration) =>
                                          {
                                              loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration)
                                                                 .Enrich.WithProperty("App Name : "  , ApplicationConstant.ApplicationName)
                                                                 .Enrich.FromLogContext().
                                                                 WriteTo.Console().
                                                                 Enrich.AtLevel(LogEventLevel.Verbose , configuration => configuration.FromLogContext()).
                                                                 WriteTo.Console().
                                                                 WriteTo.OpenTelemetry(options =>
                                                                                       {
                                                                                           options.Headers = ApplicationConstant.Aspire.HeaderKey;
                                                                                       });
                                          });
        }
        
        
    }
}