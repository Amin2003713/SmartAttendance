using System.Collections.Generic;

namespace Shifty.Domain.Constants
{
    public static class ApplicationConstant
    {
        public static class Sql
        {
            public const string DbServer                 = "sqlserver";
            public const string MultipleActiveResultSets = "MultipleActiveResultSets=true";
            public const string Encrypt                  = "Encrypt=false";

            public const string UserNameAndPass =
                "User Id=sa;Password=8CCA6B60A3EF40A59128@6D2824C9AEDC";
        }
        public static class Identity
        {
            public const string CodeGenerator = "THERE WEAR ONES A CHILE THAT DIED HAHA .";

        }
        public static class Mino
        {
            public const string MinioEndpoint  = "http://shiftyMinioFileDb:9000";
            public const string MinioAccessKey = "DwCnnRFdBHjVgNCFWZM8";
            public const string MinioSecretKey = "xqLBtGiy9sqnwSgZboADOqJ6MP6nBowbnB35bbKA";
        }

        public static class Aspire
        {
            public const           string                      Header    = "x-otlp-api-key=FC83FFEF-1C71-4C88-97D7-27CE9570F131";
            public readonly static Dictionary<string , string> HeaderKey = new Dictionary<string , string>
            {
                {
                    "x-otlp-api-key" , "FC83FFEF-1C71-4C88-97D7-27CE9570F131"
                } 
            };
        }
        public const string ApplicationName       = "Shifty";
    }
}