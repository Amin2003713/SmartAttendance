using System.Collections.Generic;

namespace Shifty.Domain.Constants
{
    public static class ApplicationConstant
    {
        public const string ApplicationName = "Shifty";

        public static class Sql
        {
            public const string DbServer                 = "sqlserver,1433";
            public const string MultipleActiveResultSets = "MultipleActiveResultSets=true";
            public const string Encrypt                  = "Encrypt=false";

            public const string UserNameAndPass = "User Id=sa;Password=8CCA6B60A3EF40A59128@6D2824C9AEDC";
        }

        public static class Identity
        {
            public const string CodeGenerator = "THERE WEAR ONES A CHILE THAT DIED HAHA .";
        }

        public static class Mino
        {
            public const string MinioEndpoint  = "http://shiftyMinioFileDb:9000";
            public const string MinioAccessKey = "r6dE962QPcpXw5ty2OwY";
            public const string MinioSecretKey = "PsBcq5CPmHNYxhqi7lf8Ox9wjKfqfmwgT7EYPFg4";
        }

        public static class Aspire
        {
            public const string Header = "x-otlp-api-key=FC83FFEF-1C71-4C88-97D7-27CE9570F131";

            public readonly static Dictionary<string , string> HeaderKey = new Dictionary<string , string>
            {
                {
                    "x-otlp-api-key" , "FC83FFEF-1C71-4C88-97D7-27CE9570F131"
                } ,
            };
        }

        public static class AppOptions
        {
            public static string WriteDatabaseConnectionString { get; set; } = $"Server={Sql.DbServer};Database=Shifty.Shard;{Sql.MultipleActiveResultSets};{Sql.
                Encrypt};{Sql.UserNameAndPass}";

            public static string ReadDatabaseConnectionString { get; set; } = $"Server={Sql.DbServer};Database=Shifty.Shard;{Sql.MultipleActiveResultSets};{Sql.
                Encrypt};{Sql.UserNameAndPass}";

            public static string TenantStore { get; set; } = $"Server={Sql.DbServer};Database=Shifty;{Sql.MultipleActiveResultSets};{Sql.
                Encrypt};{Sql.UserNameAndPass}";

            public static string RedisConnectionString { get; set; } = "localhost:6379";

            public static string AuthenticationServerUri { get; set; } = "";

            public static bool ActivateSwagger { get; set; } = true;

            public static List<string> CorsEnableUris { get; set; } = [];
        }
    }
}