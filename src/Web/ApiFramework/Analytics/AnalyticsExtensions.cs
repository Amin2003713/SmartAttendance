using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Exceptions;
using SmartAttendance.Common.General;

namespace SmartAttendance.ApiFramework.Analytics;

public static class AnalyticsExtensions
{
    private readonly static ActivitySource ActivitySource = new(Assembly.GetCallingAssembly().GetName().ToString());

    private readonly static ResourceBuilder ResourceBuilder =
        ResourceBuilder.CreateDefault().AddService(Assembly.GetCallingAssembly().GetName().ToString());


    private static void AddObservabilityServices(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithMetrics(builder =>
            {
                builder.SetResourceBuilder(ResourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddMeter(Assembly.GetCallingAssembly().GetName() + "Metrics")
                    .AddOtlpExporter(ApplicationConstant.Aspire.OtlpExporter);
            })
            .WithTracing(builder =>
            {
                builder.SetResourceBuilder(ResourceBuilder)
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.EnrichWithHttpRequest = (activity, req) =>
                        {
                            activity.SetTag("correlation_id",
                                req.Headers["X-Correlation-ID"].ToString());

                            activity.SetTag("client_ip",
                                req.HttpContext.Connection.RemoteIpAddress?.ToString());
                        };

                        options.EnrichWithHttpResponse = (activity, resp) =>
                        {
                            activity.SetTag("http.status_code",
                                resp.StatusCode);
                        };
                    })
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation(opts => opts.SetDbStatementForText = true)
                    .AddRedisInstrumentation(options =>
                    {
                        // capture the Redis command text in the span
                        options.SetVerboseDatabaseStatements = true;
                        options.EnrichActivityWithTimingEvents = true;
                    })
                    .AddSource(ActivitySource.Name)
                    .AddOtlpExporter(ApplicationConstant.Aspire.OtlpExporter);
            });

        services.AddTransient<CorrelationIdMiddleware>();
    }


    public static void AddSerilogLogging(this IServiceCollection services)
    {
        services.AddObservabilityServices();

        var loggerFactory = new LoggerConfiguration().Enrich
            .WithProperty("Application : ", Assembly.GetCallingAssembly().GetName())
            .Enrich.WithCorrelationId()
            .Enrich
            .WithClientIp()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console(outputTemplate:
                "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{Exception}{NewLine}")
            .WriteTo
            .OpenTelemetry(conf =>
            {
                conf.Headers = ApplicationConstant.Aspire.HeaderKey;
                conf.Endpoint = ApplicationConstant.Aspire.OtelEndpoint;
            })
            .CreateLogger();

        services.AddSingleton(loggerFactory);
    }

    public static IHostBuilder UseSerilogLogging(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((_, loggerConfiguration) =>
        {
            loggerConfiguration.Enrich.WithProperty("Application : ", Assembly.GetCallingAssembly().GetName())
                .Enrich
                .WithCorrelationId()
                .Enrich.WithClientIp()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{Exception}{NewLine}"
                )
                .WriteTo.OpenTelemetry(conf =>
                {
                    conf.Headers = ApplicationConstant.Aspire.HeaderKey;
                    conf.Endpoint = ApplicationConstant.Aspire.OtelEndpoint;
                });
        });
    }
}