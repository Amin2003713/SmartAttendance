using System.Collections.Generic;
using Finbuckle.MultiTenant;
using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;
using Shifty.ApiFramework.Analytics;
using Shifty.ApiFramework.Middleware.Localaizer;
using Shifty.ApiFramework.Middleware.Tenant;
using Shifty.Common.General;
using Shifty.Persistence.Services.Taskes;

namespace Shifty.ApiFramework.Injections;

public static class WebAppBuilderExtensions
{
    public static void UseWebApi<TResourceUsage>(
        this IApplicationBuilder app,
        string scalarTitle = "API Reference",
        string? ScalarRoutTemp = null!)
    {
        app.UseCors(builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });

        app.UseLocalaizer();

        app.UseMultiTenant();
        app.UseMiddleware<TenantValidationMiddleware>();
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<HandelArchive>();

        app.UseAppSwagger();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.ConfigureAutomaticTasks();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.MapScalarApiReference(opt =>
            {
                opt.OpenApiRoutePattern = (ApplicationConstant.AppOptions.GetSwaggerPath() ??
                                           ScalarRoutTemp)!;

                opt.Servers = [];
                opt.Title = scalarTitle;
                opt.Theme = ScalarTheme.BluePlanet;
                opt.Layout = ScalarLayout.Classic;

                opt.DefaultHttpClient
                    = new KeyValuePair<ScalarTarget, ScalarClient>(ScalarTarget.Http, ScalarClient.Axios);

                opt.DarkMode = true;

                opt.EnabledClients = new[]
                {
                    ScalarClient.Axios,
                    ScalarClient.Http2,
                    ScalarClient.Httpie,
                    ScalarClient.Requests,
                    ScalarClient.Native,
                    ScalarClient.HttpClient,
                    ScalarClient.Curl
                };

                opt.HideModels = true;
                opt.OperationSorter = OperationSorter.Alpha;
                opt.DynamicBaseServerUrl = true;
                opt.ShowSidebar = true;
                opt.PersistentAuthentication = true;
            });

            endpoints.MapHangfireDashboard();


            endpoints.MapHealthChecks("/api/health",
                new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
        });

        app.UseHangfireDashboard(
            // , new DashboardOptions
            // {
            //     // Only authenticated admin users can access the dashboard.
            //     Authorization = new[]
            //     {
            //         new HangfireAuthorizationFilter()
            //     } ,
            //     // Optionally set read-only mode if needed.
            //     IsReadOnlyFunc = context =>
            //                      {
            //                          return !context.GetHttpContext().User.IsInRole("Admin");
            //                      }
            // }
        );


        if (typeof(TResourceUsage) != typeof(Empty))
            app.UseMiddleware<TResourceUsage>();
    }

    public static void ConfigureAutomaticTasks(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var tasker = scope.ServiceProvider.GetRequiredService<ScheduledTaskService>();

        tasker.ScheduleTasks();
    }

    private static void UseAppSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger(options => { options.RouteTemplate = "api/swagger/{documentName}/swagger.json"; });

        app.UseReDoc(options =>
        {
            options.EnableUntrustedSpec();
            options.ScrollYOffset(10);
            options.HideHostname();
            options.HideDownloadButton();
            options.ExpandResponses("200,201");
            options.RequiredPropsFirst();
            options.NoAutoAuth();
            options.PathInMiddlePanel();
            options.HideLoading();
            options.NativeScrollbars();
            options.OnlyRequiredInSamples();
            options.SortPropsAlphabetically();
        });
    }
}