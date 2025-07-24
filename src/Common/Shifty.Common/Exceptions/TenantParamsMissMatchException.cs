namespace Shifty.Common.Exceptions;

public class TenantParamsMissMatchException : Exception
{
    public TenantParamsMissMatchException()
        : base("One or more tenant parameters are missing.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public TenantParamsMissMatchException(string message)
        : base(message) { }

    public TenantParamsMissMatchException(string message, Exception innerException)
        : base(message, innerException) { }

    public IDictionary<string, string[]> Errors { get; set; }
}