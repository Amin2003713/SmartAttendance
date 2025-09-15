using System.Net;

namespace SmartAttendance.Common.Exceptions;

/// <summary>
///     Represents an HTTP 403 Forbidden error.
/// </summary>
public class ForbiddenException(
    string    message,
    Exception innerException,
    object    additionalData = null
) : SmartAttendanceException(
    message,
    HttpStatusCode.Forbidden,
    innerException,
    additionalData);