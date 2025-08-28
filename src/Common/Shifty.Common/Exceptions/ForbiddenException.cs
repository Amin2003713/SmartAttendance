using System.Net;

namespace Shifty.Common.Exceptions;

/// <summary>
///     Represents an HTTP 403 Forbidden error.
/// </summary>
public class ForbiddenException(
    string message,
    Exception innerException,
    object additionalData = null
) : ShiftyException(
    message,
    HttpStatusCode.Forbidden,
    innerException,
    additionalData);