using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;

namespace SmartAttendance.ApiFramework.Middleware.Localaizer;

public static class Localaizer
{
    public static IApplicationBuilder UseLocalaizer(this IApplicationBuilder app)
    {
        app.UseRequestLocalization(options =>
        {
            options.RequestCultureProviders.Clear();

            IList<CultureInfo> supportedCultures =
            [
                new("fa-IR")
                {
                    DateTimeFormat =
                    {
                        Calendar = new GregorianCalendar()
                    }
                },
                new("en-US")
                {
                    DateTimeFormat =
                    {
                        Calendar = new GregorianCalendar()
                    }
                }
            ];

            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.DefaultRequestCulture = new RequestCulture("fa-IR");
            options.RequestCultureProviders.Insert(0,
                new CustomRequestCultureProvider(context =>
                {
                    var culture = context.Request.Headers["X-Accept-Language"].ToString().Split(',').FirstOrDefault();

                    if (!string.IsNullOrEmpty(culture) &&
                        supportedCultures.Any(c => c.Name.Equals(culture,
                            StringComparison.OrdinalIgnoreCase)) &&
                        culture != "*")
                        return Task.FromResult(new ProviderCultureResult(culture));

                    return Task.FromResult(new ProviderCultureResult("fa-IR"));
                }));
        });

        return app;
    }
}