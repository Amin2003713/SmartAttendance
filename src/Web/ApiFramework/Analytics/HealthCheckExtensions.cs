using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using Shifty.Common.General;

namespace Shifty.ApiFramework.Analytics;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck("self",
                () => HealthCheckResult.Healthy(),
                ["live", "internal"])
            .AddSqlServer(
                ApplicationConstant.AppOptions.TenantStore,
                name: "sqlserver",
                tags: ["db", "sql", "critical"])
            .AddMongoDb(
                _ =>
                {
                    var mongoUrlBuilder = new MongoUrlBuilder
                    {
                        Server = new MongoServerAddress(ApplicationConstant.Mongo.Host, ApplicationConstant.Mongo.Port),
                        DatabaseName = ApplicationConstant.Mongo.DefaultDb,
                        Username = ApplicationConstant.Mongo.UserName,
                        Password = ApplicationConstant.Mongo.Password,
                        AuthenticationSource = "admin",
                        AllowInsecureTls = true
                    };


                    var client = new MongoClient(mongoUrlBuilder.ToMongoUrl());
                    return client.GetDatabase(ApplicationConstant.Mongo.DefaultDb);
                },
                "mongodb",
                tags: new[]
                {
                    "db",
                    "nosql",
                    "critical"
                })
            .AddRedis(
                ApplicationConstant.AppOptions.RedisConnectionString,
                "redis",
                tags: new[]
                {
                    "cache",
                    "infra"
                })
            .AddUrlGroup(
                new Uri($"{ApplicationConstant.Minio.Endpoint}/minio/health/live"),
                name: "minio",
                httpMethod: HttpMethod.Get,
                tags: new[]
                {
                    "storage",
                    "s3",
                    "infra"
                });


        return services;
    }
}