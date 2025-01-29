using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Correlate;
using Serilog.Events;
using Shifty.Api.Services;
using System;
using System.Linq;
using Shifty.Common.General;

namespace Shifty.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).
                        UseServiceProviderFactory(new AutofacServiceProviderFactory()).
                        ConfigureWebHostDefaults(webBuilder =>
                                                 {
                                                     webBuilder.UseStartup<Startup>();
                                                 }).
                        UseSerilog(ConfigureLogger);
        }

        private static void ConfigureLogger(HostBuilderContext hostBuilderContext , IServiceProvider serviceProvider , LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration).
                                Enrich.FromLogContext().
                                Enrich.AtLevel(LogEventLevel.Verbose , configuration => configuration.FromLogContext()).
                                WriteTo.Console().
                                WriteTo.OpenTelemetry(options =>
                                                      {
                                                          options.Headers = ApplicationConstant.Aspire.HeaderKey;
                                                      });
        }
    }
}