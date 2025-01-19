using System;
using System.Collections.Generic;

namespace Shifty.Common.Exceptions
{
    public class ExistingRecordException : Exception
    {
        public ExistingRecordException()
        {
        }

        public ExistingRecordException(string message)
            : base(message)
        {
        }

        public ExistingRecordException(string message , Exception innerException)
            : base(message , innerException)
        {
        }

        public ExistingRecordException(string name , object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }

    public class TenantParamsMissMatchException : Exception
    {
        public TenantParamsMissMatchException()
            : base("One or more tenant parameters are missing.")
        {
            Errors = new Dictionary<string , string[]>();
        }

        public TenantParamsMissMatchException(string message)
            : base(message)
        {
        }

        public TenantParamsMissMatchException(string message , Exception innerException)
            : base(message , innerException)
        {
        }

        public IDictionary<string , string[]> Errors { get; set; }
    }
}