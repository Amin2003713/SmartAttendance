using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Shifty.Domain.Constants;
using System;
using System.Collections.Generic;

namespace Shifty.ApiFramework.Aspire
{
    public static class AspireExtensions
    {
        public static IServiceCollection AddAspire(this IServiceCollection services)
        {

            services.Configure<OpenTelemetryLoggerOptions>(options =>
                                                           {
                                                               options.IncludeFormattedMessage = true;
                                                               options.IncludeScopes           = true;
                                                           });


  services.AddLogging(loggingBuilder =>
                                {
                                    loggingBuilder.ClearProviders(); // Optional: Remove other logging providers

                                    loggingBuilder.AddOpenTelemetry(logging =>
                                                                    {
                                                                        // Include formatted messages and scopes
                                                                        logging.IncludeFormattedMessage = true;
                                                                        logging.IncludeScopes           = true;

                                                                        // Define resource attributes (e.g., service name, environment)
                                                                        logging.SetResourceBuilder(ResourceBuilder.CreateDefault().
                                                                            AddService(ApplicationConstant.ApplicationName));

                                                                        // Add OTLP Exporter for Aspire
                                                                        logging.AddOtlpExporter(options =>
                                                                                                {
                                                                                                    options.Headers =
                                                                                                        "x-otlp-api-key=FC83FFEF-1C71-4C88-97D7-27CE9570F131";
                                                                                                });

                                                                        // Optional: Add Console Exporter for local debugging
                                                                        logging.AddConsoleExporter();

                                                                    });

                                    // Optional: Add other logging providers if needed
                                    loggingBuilder.AddDebug();
                                });



            services.AddMetrics().
                     AddOpenTelemetry().
                     ConfigureResource(c => c.AddService(nameof(Shifty))).
                     WithMetrics(builder =>
                                 {
                                     builder.AddAspNetCoreInstrumentation().
                                             AddProcessInstrumentation().
                                             AddRuntimeInstrumentation().
                                             AddHttpClientInstrumentation();

                                     builder.AddOtlpExporter(option =>
                                                             {
                                                                 option.Headers = "x-otlp-api-key=FC83FFEF-1C71-4C88-97D7-27CE9570F131";
                                                             });
                                 }).
                     WithTracing(builder =>
                                 {
                                     builder.AddAspNetCoreInstrumentation().
                                             AddHttpClientInstrumentation().
                                             AddEntityFrameworkCoreInstrumentation().
                                             AddHangfireInstrumentation();

                                     builder.AddOtlpExporter(option =>
                                                             {
                                                                 option.Headers = "x-otlp-api-key=FC83FFEF-1C71-4C88-97D7-27CE9570F131";
                                                             });
                                 }).
                     WithLogging(builder =>
                                 {
                                     builder.AddOtlpExporter(option =>
                                                             {
                                                                 option.Headers = "x-otlp-api-key=FC83FFEF-1C71-4C88-97D7-27CE9570F131";
                                                             });
                                 });

            return services;
        }
    }
}