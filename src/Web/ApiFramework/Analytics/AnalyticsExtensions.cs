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

            services.AddSerilogLogging();

            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddService(ApplicationConstant.ApplicationName);

            services.AddOpenTelemetry()
                .WithMetrics(builder =>
                {
                    builder
                        .SetResourceBuilder(resourceBuilder)
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddRuntimeInstrumentation()
                        .AddProcessInstrumentation()
                        .AddMeter (ApplicationConstant.ApplicationName + "Metrics").
                        AddOtlpExporter(ApplicationConstant.Aspire.OtlpExporter);
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

        private static void AddSerilogLogging(this IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog();
            });
        }

        public static IHostBuilder UseSerilogLogging(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseSerilog((_, loggerConfiguration) =>
            {
                loggerConfiguration
                    .Enrich.WithProperty("Application : ", ApplicationConstant.ApplicationName)
                    .Enrich.WithMachineName()
                    .Enrich.WithThreadId()
                    .Enrich.WithProcessId()
                    .Enrich.WithCorrelationId()
                    .Enrich.FromLogContext()
                    .Enrich.WithClientIp()
                    .Enrich.WithExceptionDetails()
                    .Enrich.WithThreadName().
                    WriteTo.Console(
                        outputTemplate:
                        "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj} (Thread: {ThreadId}, Machine: {MachineName}){NewLine}{Exception}")
                    .WriteTo.OpenTelemetry(conf =>
                                           {
                                               conf.Headers = ApplicationConstant.Aspire.HeaderKey;
                                               conf.Endpoint = ApplicationConstant.Aspire.OtelEndpoint;
                                           })
                    // .WriteTo.File("logs/shifty.log", rollingInterval: RollingInterval.Day)
                    ;
            });
        }
    }

    public class CorrelationIdMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
            {
                correlationId = Guid.CreateVersion7().ToString();
                context.Request.Headers["X-Correlation-ID"] = correlationId;
            }

            context.Response.Headers["X-Correlation-ID"] = correlationId;

            var activity = Activity.Current;
            activity?.SetTag("correlation_id", correlationId.ToString());

            await next(context);
        }
    }


}
