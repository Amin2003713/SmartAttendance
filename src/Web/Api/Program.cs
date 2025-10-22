using Autofac.Extensions.DependencyInjection;
using Ipa.Framework.Apis.Analytics;
using Microsoft.Extensions.Hosting;
using SmartAttendance.Api.Services;

namespace SmartAttendance.Api;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseSerilogLogging()
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}