using OpenTelemetry.Exporter;

namespace SmartAttendance.Common.General;

public abstract class ApplicationConstant
{
    public readonly static bool IsDevelopment =
        (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production") == "Development";


    public static string ServiceName => Assembly.GetEntryAssembly()!.GetName().Name?.ToLowerInvariant()?.Split(".")[0] ?? "service";

    public static class Aspire
    {
        public readonly static string OtelEndpoint = (  "http://10.0.2.2:17011")!;


        public static Action<OtlpExporterOptions> OtlpExporter => options =>
        {
            options.Endpoint = new Uri(OtelEndpoint);
        };
    }

    public static class Sql
    {
        public const string DbServer                 = "sqlserver,1433";
        public const string MultipleActiveResultSets = "MultipleActiveResultSets=true";
        public const string Encrypt                  = "Encrypt=false";
        public const string UserNameAndPass          = "User Id=sa;Password=8CCA6B60A3EF40A59128@6D2824C9AEDC";
    }

    public static class Identity
    {
        public const string CodeGenerator = "THERE WAS ONCE A CHILD THAT DIED, HAHA.";
    }

    public static class Minio
    {
        public static string Endpoint { get; set; } = "http://minio:9000";
        public static string AccessKey { get; set; } = "gGdVCDO72q74X9lEKfuz";
        public static string SmFont { get; set; } = "Sm/B_Zar.ttf";
        public static string SecretKey { get; set; } = "VLNkSksKp5S7nL8F684qkiBTgMnE5amlKQDTtwQz";
        public static string SmLoge { get; set; } = "Sm/newSmLogo.png";
    }


    public static class AppOptions
    {
        public static string WriteDatabaseConnectionString { get; set; } =
            $"Server={Sql.DbServer};Database=SmartAttendance.Shard;{Sql.MultipleActiveResultSets};{Sql.Encrypt};{Sql.UserNameAndPass}";

        public static string ReadDatabaseConnectionString { get; set; } =
            $"Server={Sql.DbServer};Database=SmartAttendance.Shard;{Sql.MultipleActiveResultSets};{Sql.Encrypt};{Sql.UserNameAndPass}";

        public static string TenantStore { get; set; } =
            $"Server={Sql.DbServer};Database=SmartAttendance;{Sql.MultipleActiveResultSets};{Sql.Encrypt};{Sql.UserNameAndPass}";

        public static string RedisConnectionString { get; set; } =
            "redis:6379,password=4f7d8a7e6b5c9f4b2a1d3e5f8c0b7a8CCA6B60A3EF40A59128@6@Sm";

        public static string GetSwaggerPath()
        {
            return ServiceName.Equals("apigateway", StringComparison.OrdinalIgnoreCase)
                ? "api/swagger/swagger.json"
                : "api/swagger/v1/swagger.json";
        }
    }

    public static class JwtSettings
    {
        public static string SecretKey { get; set; } =
            "LongerThan-16Char-SecretKey79D27861E443-EA37-43D5-B566-FCBCB79D2786";

        public static string Issuer { get; set; } =
            "Ipa.Back@79D27861E443-EA37-43D5-B566-FCBCB79D2786";

        public static string Audience { get; set; } =
            "Ipa.Client@79D27861E443-EA37-43D5-B566-FCBCB79D2786";

        public static int NotBeforeMinutes { get; set; } = 0;
        public static int ExpirationMinutes { get; set; } = 1440;
        public static int RefreshTokenValidityInDays { get; set; } = 30;
    }

    public static class Const
    {
        public readonly static string EmailSuffix = "@gmail.com";
        public readonly static string BaseDomain  = Environment.GetEnvironmentVariable("BASE_URL") ?? "mrShift.ir";
    }
}