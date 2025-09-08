using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace SmartAttendance.ApiFramework.Configuration;

public class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value == null) return null;

        return Regex.Replace(value.ToString()!, "_", "-");
    }
}