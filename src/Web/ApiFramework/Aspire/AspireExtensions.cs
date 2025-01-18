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

    public static IServiceCollection AddAspire(this IServiceCollection services)
    {

        services.Configure<OpenTelemetryLoggerOptions>(options =>
                                                       {
                                                           options.IncludeFormattedMessage = true;
                                                           options.IncludeScopes           = true;
                                                       });


        services.AddOpenTelemetry().
                 ConfigureResource(builder => builder.AddService(ApplicationConstant.ApplicationName)).
                 WithMetrics(builder =>
                             {
                                 builder.AddHttpClientInstrumentation().
                                         AddProcessInstrumentation().
                                         AddRuntimeInstrumentation().
                                         AddAspNetCoreInstrumentation();

                                 builder.AddOtlpExporter(option =>
                                                         {
                                                             option.Headers  = $"x-otlp-api-key=FC83FFEF-1C71-4C88-97D7-27CE9570F131";
                                                         });
                             }).
                 WithTracing(builder =>
                             {
                                 builder.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddEntityFrameworkCoreInstrumentation();

                                 builder.AddOtlpExporter(option =>
                                                         {
                                                             option.Headers  = $"x-otlp-api-key=FC83FFEF-1C71-4C88-97D7-27CE9570F131";
                                                         });
                             });

        services.AddLogging(loggingBuilder =>
                            {
                                loggingBuilder.AddConsole();
                                loggingBuilder.AddOpenTelemetry(options =>
                                                                {
                                                                    options.AddOtlpExporter(option =>
                                                                                            {
                                                                                                option.Headers =
                                                                                                    "x-otlp-api-key=FC83FFEF-1C71-4C88-97D7-27CE9570F131";
                                                                                            });
                                                                });
                            });

        return services;
    }             
}