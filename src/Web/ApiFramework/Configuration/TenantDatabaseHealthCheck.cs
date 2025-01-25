using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Shifty.Persistence.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.ApiFramework.Configuration;

public class TenantDatabaseHealthCheck(IServiceProvider serviceProvider) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var healthReport     = new Dictionary<string, object>();
        var unhealthyTenants = new List<string>();

        try
        {
            using var scope       = serviceProvider.CreateScope();
            var       tenantStore = scope.ServiceProvider.GetRequiredService<TenantDbContext>();

            // Get all tenant configurations
            var tenants = await tenantStore.TenantInfo.ToListAsync(cancellationToken);

            foreach (var tenant in tenants)
            {
                var tenantStatus = new Dictionary<string, object>
                {
                    { "ConnectionString", tenant.GetConnectionString() },
                    { "HealthStatus", "Healthy" }
                };

                var             connectionString = tenant.GetConnectionString();
                await using var connection       = new SqlConnection(connectionString);

                try
                {
                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                    await connection.OpenAsync(cancellationToken);
                    stopwatch.Stop();

                    tenantStatus["ResponseTimeMs"] = stopwatch.ElapsedMilliseconds;
                }
                catch (SqlException ex)
                {
                    tenantStatus["HealthStatus"] = "Unhealthy";
                    tenantStatus["Error"]        = ex.Message;
                    unhealthyTenants.Add(tenant.Name);
                }
                catch (Exception ex)
                {
                    tenantStatus["HealthStatus"] = "Unhealthy";
                    tenantStatus["Error"]        = $"Unexpected error: {ex.Message}";
                    unhealthyTenants.Add(tenant.Name);
                }
                finally
                {
                    await connection.CloseAsync();
                }

                healthReport[tenant.Name] = tenantStatus;
            }

            var allHealthy = !unhealthyTenants.Any();
            var status     = allHealthy ? HealthStatus.Healthy : HealthStatus.Degraded;

            return new HealthCheckResult(
                status,
                description: allHealthy
                    ? "All tenant databases are healthy."
                    : $"Some tenant databases are unhealthy: {string.Join(", ", unhealthyTenants)}",
                data: healthReport
            );
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(
                HealthStatus.Unhealthy,
                description: $"Error while checking tenant databases: {ex.Message}",
                data : new Dictionary<string , object>()
                {
                    {
                        ex.Source! , ex
                    }
                }
            );
        }
    }

}