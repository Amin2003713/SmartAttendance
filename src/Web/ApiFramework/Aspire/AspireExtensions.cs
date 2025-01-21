using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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


            services.AddLogging(loggingBuilder =>
                                {
                                    loggingBuilder.AddOpenTelemetry(logging =>
                                                                    {
                                                                        logging.IncludeFormattedMessage = true;
                                                                        logging.IncludeScopes           = true;
                                                                        logging.AddOtlpExporter(option =>
                                                                                                {
                                                                                                    option.Headers =
                                                                                                        "x-otlp-api-key=FC83FFEF-1C71-4C88-97D7-27CE9570F131";
                                                                                                });
                                                                    });
                                });

            return services;
        }
    }
}