using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Shifty.Api.Services;
using Shifty.Domain.Constants;

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
                        UseSerilog((hostBuilderContext , loggerConfiguration) =>
                                   {
                                       loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration).
                                                           Enrich.FromLogContext().
                                                           WriteTo.Console().
                                                           WriteTo.OpenTelemetry(options =>
                                                                                 {
                                                                                     options.Headers = ApplicationConstant.Aspire.HeaderKey;
                                                                                 });
                                   }).
                        ConfigureWebHostDefaults(webBuilder =>
                                                 {
                                                     webBuilder.UseStartup<Startup>();
                                                 });
        }
    }
}