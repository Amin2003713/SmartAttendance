using System;
using System.Collections.Generic;
using OpenTelemetry.Exporter;

namespace Shifty.Common.General
{
    public static class ApplicationConstant
    {
        public const string ApplicationName = "Shifty";

        public static readonly bool IsDevelopment = 
            (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production") == "Development";

        public static class Sql
        {
            public const string DbServer = "sqlserver,1433";
            public const string MultipleActiveResultSets = "MultipleActiveResultSets=true";
            public const string Encrypt = "Encrypt=false";
            public const string UserNameAndPass = "User Id=sa;Password=8CCA6B60A3EF40A59128@6D2824C9AEDC";
        }

        public static class Identity
        {
            public const string CodeGenerator = "THERE WAS ONCE A CHILD THAT DIED, HAHA.";
        }

        public static class Minio
        {
            public const string Endpoint = "http://shiftyMinioFileDb:9000";
            public const string AccessKey = "r6dE962QPcpXw5ty2OwY";
            public const string SecretKey = "PsBcq5CPmHNYxhqi7lf8Ox9wjKfqfmwgT7EYPFg4";
        }

        public static class Aspire
        {
            public static Action<OtlpExporterOptions> OtlpExporter => options =>
            {
                options.Endpoint = new Uri(OtelEndpoint);
                if (!string.IsNullOrEmpty(Header))
                {
                    options.Headers = Header;
                }
            };

            public const string Header = "x-otlp-api-key=FC83FFEF-1C71-4C88-97D7-27CE9570F131";

            public readonly static string OtelEndpoint =  "http://shiftyAspireService:18889";


            public readonly static Dictionary<string , string> HeaderKey = new Dictionary<string , string>
            {
                {
                    "x-otlp-api-key" , "FC83FFEF-1C71-4C88-97D7-27CE9570F131"
                } ,
            };
        }

        public static class AppOptions
        {
            public static string WriteDatabaseConnectionString { get; set; } = 
                $"Server={Sql.DbServer};Database=Shifty.Shard;{Sql.MultipleActiveResultSets};{Sql.Encrypt};{Sql.UserNameAndPass}";

            public static string ReadDatabaseConnectionString { get; set; } = 
                $"Server={Sql.DbServer};Database=Shifty.Shard;{Sql.MultipleActiveResultSets};{Sql.Encrypt};{Sql.UserNameAndPass}";

            public static string TenantStore { get; set; } = 
                $"Server={Sql.DbServer};Database=Shifty;{Sql.MultipleActiveResultSets};{Sql.Encrypt};{Sql.UserNameAndPass}";

            public static string RedisConnectionString { get; set; } = "localhost:6379";
            public static string AuthenticationServerUri { get; set; } = "";
            public static bool ActivateSwagger { get; set; } = true;
            public static List<string> CorsEnableUris { get; set; } = [];
        }

        public static class JwtSettings
        {
            public static string SecretKey { get; set; } = 
                "LongerThan-16Char-SecretKey79D27861E443-EA37-43D5-B566-FCBCB79D2786";

            public static string Issuer { get; set; } = 
                "Shifty.Back@79D27861E443-EA37-43D5-B566-FCBCB79D2786";

            public static string Audience { get; set; } = 
                "Shifty.Client@79D27861E443-EA37-43D5-B566-FCBCB79D2786";

            public static int NotBeforeMinutes { get; set; } = 0;
            public static int ExpirationMinutes { get; set; } = 5;
            public static int RefreshTokenValidityInDays { get; set; } = 30;
        }
    }
}
