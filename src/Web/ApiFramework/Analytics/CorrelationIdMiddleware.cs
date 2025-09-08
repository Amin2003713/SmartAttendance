using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SmartAttendance.ApiFramework.Analytics;

public class CorrelationIdMiddleware(
    RequestDelegate next
)
{
    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
        {
            correlationId = Guid.CreateVersion7().ToString();
            context.Request.Headers["X-Correlation-ID"] = correlationId;
        }

        context.Response.Headers["X-Correlation-ID"] = correlationId;

        var activity = Activity.Current;
        activity?.SetTag("correlation_id", correlationId.ToString());

        await next(context);
    }
}