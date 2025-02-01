using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers.Correlate;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.File;
using Shifty.Common.General;

namespace Shifty.ApiFramework.Analytics
{
    public static class AnalyticsExtensions
    {
        private readonly static ActivitySource ActivitySource = new(ApplicationConstant.ApplicationName);

        public static IServiceCollection AddObservabilityServices(this IServiceCollection services)
        {
            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddService(ApplicationConstant.ApplicationName)
                .AddAttributes(new Dictionary<string, object>
                {
                    { "environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production" },
                    { "host", Environment.MachineName }
                });

            services.AddOpenTelemetry()
                .WithMetrics(builder =>
                {
                    builder
                        .SetResourceBuilder(resourceBuilder)
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddRuntimeInstrumentation()
                        .AddProcessInstrumentation()
                        .AddMeter("Shifty.ApiMetrics"); // Custom Business Metrics
                })
                .WithTracing(builder =>
                {
                    builder
                        .SetResourceBuilder(resourceBuilder)
                        .AddAspNetCoreInstrumentation(options =>
                        {
                            options.EnrichWithHttpRequest = (activity, httpRequest) =>
                            {
                                activity.SetTag("correlation_id", httpRequest.Headers["X-Correlation-ID"].ToString());
                                activity.SetTag("client_ip", httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString());
                                activity.SetTag("request_content_length", httpRequest.ContentLength);
                            };

                            options.EnrichWithHttpResponse = (activity, httpResponse) =>
                            {
                                activity.SetTag("http.status_code", httpResponse.StatusCode);
                                activity.SetTag("response_content_length", httpResponse.ContentLength);
                            };
                        })
                        .AddHttpClientInstrumentation()
                        .AddSqlClientInstrumentation(options =>
                        {
                            options.SetDbStatementForText = true; // Logs SQL queries
                        })
                        .AddSource(ActivitySource.Name)
                        .AddOtlpExporter(ApplicationConstant.Aspire.OtlpExporter);
                });

            services.AddTransient<CorrelationIdMiddleware>();

            return services;
        }

        public static IServiceCollection AddSerilogLogging(this IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog();
            });

            return services;
        }

        public static IHostBuilder UseSerilogLogging(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseSerilog((_, loggerConfiguration) =>
            {
                loggerConfiguration
                    .Enrich.WithProperty("AppName", ApplicationConstant.ApplicationName)
                    .Enrich.WithMachineName()
                    .Enrich.WithThreadId()
                    .Enrich.WithProcessId()
                    .Enrich.WithCorrelationId() // Logs full stack trace for exceptions
                    .Enrich.FromLogContext()
                    .Enrich.WithClientIp()
                    .Enrich.WithExceptionDetails()
                    .Enrich.WithThreadName().
                    WriteTo.Console(
                        outputTemplate:
                        "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj} (Thread: {ThreadId}, Machine: {MachineName}){NewLine}{Exception}")
                    .WriteTo.Seq("http://seq:80")
                    .WriteTo.File("logs/shifty.log", rollingInterval: RollingInterval.Day);
            });
        }
    }

    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers["X-Correlation-ID"] = correlationId;
            }

            context.Response.Headers["X-Correlation-ID"] = correlationId;

            var activity = Activity.Current;
            if (activity != null)
            {
                activity.SetTag("correlation_id", correlationId.ToString());
            }

            await _next(context);
        }
    }

    public static class SerilogEnrichmentExtensions
    {
        public static LoggerConfiguration WithCorrelationId(this LoggerConfiguration loggerConfiguration)
        {
            return loggerConfiguration.Enrich.With(new CorrelationIdEnricher());
        }
    }

    public class CorrelationIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var correlationId = Activity.Current?.GetTagItem("correlation_id")?.ToString();
            if (!string.IsNullOrEmpty(correlationId))
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CorrelationId", correlationId));
            }
        }
    }
}
