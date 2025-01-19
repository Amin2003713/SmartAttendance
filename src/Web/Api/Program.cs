using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Shifty.Api.Services;

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
                                       loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration);
                                   }).
                        ConfigureWebHostDefaults(webBuilder =>
                                                 {
                                                     webBuilder.UseStartup<Startup>();
                                                 });
        }
    }
}