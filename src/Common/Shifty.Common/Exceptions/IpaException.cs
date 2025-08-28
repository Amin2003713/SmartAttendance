using System.Net;

namespace Shifty.Common.Exceptions;

public class ShiftyException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ShiftyException" /> class with specified parameters.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    /// <param name="innerException">The inner exception.</param>
    /// <param name="additionalData">Any additional data related to the exception.</param>
    public ShiftyException(
        string message = null,
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError,
        Exception innerException = null,
        object additionalData = null)
        : base(message, innerException)
    {
        HttpStatusCode = httpStatusCode;
        AdditionalData = additionalData;
    }

    public HttpStatusCode HttpStatusCode { get; }
    public object AdditionalData { get; }

    // Static factory methods for common scenarios

    /// <summary>
    ///     Creates a DRPException with a message.
    /// </summary>
    public static ShiftyException Create(string message)
    {
        return new ShiftyException(message);
    }

    /// <summary>
    ///     Creates a DRPException with a message and additional data.
    /// </summary>
    public static ShiftyException Create(string message, object additionalData)
    {
        return new ShiftyException(message, additionalData: additionalData);
    }

    /// <summary>
    ///     Creates a DRPException with a specific HTTP status code and message.
    /// </summary>
    public static ShiftyException Create(HttpStatusCode httpStatusCode, string message)
    {
        return new ShiftyException(httpStatusCode: httpStatusCode, message: message);
    }

    /// <summary>
    ///     Creates a DRPException with all parameters.
    /// </summary>
    public static ShiftyException Create(
        string message,
        HttpStatusCode httpStatusCode,
        Exception innerException,
        object additionalData)
    {
        return new ShiftyException(message, httpStatusCode, innerException, additionalData);
    }

#region Predefined API Exceptions

    /// <summary>
    ///     Creates a NotFound DRPException (HTTP 404).
    /// </summary>
    public static NotFoundException NotFound(string message = "Resource not found.", object additionalData = null)
    {
        return additionalData is null ? new NotFoundException(message) : new NotFoundException(message, additionalData);
    }

    /// <summary>
    ///     Creates an Unauthorized DRPException (HTTP 401).
    /// </summary>
    public static UnauthorizedAccessException Unauthorized(
        string message = "Unauthorized access.",
        object additionalData = null)
    {
        return new UnauthorizedAccessException(message + "\n" + additionalData);
    }

    /// <summary>
    ///     Creates a BadRequest DRPException (HTTP 400).
    /// </summary>
    public static ShiftyException BadRequest(string message = "Bad request.", object additionalData = null)
    {
        return new ShiftyException(message, HttpStatusCode.BadRequest, null, additionalData);
    }

    /// <summary>
    ///     Creates a Validation DRPException (HTTP 400) with validation errors.
    /// </summary>
    public static ShiftyException Validation(string message = "Validation failed.", object additionalData = null)
    {
        return new ShiftyException(message, HttpStatusCode.BadRequest, null, additionalData);
    }

    /// <summary>
    ///     Creates a Conflict DRPException (HTTP 409).
    /// </summary>
    public static ConflictException Conflict(string message = "Conflict occurred.", object additionalData = null)
    {
        return new ConflictException(message, additionalData);
    }

    /// <summary>
    ///     Creates an InternalServerError DRPException (HTTP 500).
    /// </summary>
    public static ShiftyException InternalServerError(
        string message = "An unexpected error occurred.",
        object additionalData = null)
    {
        return new ShiftyException(message, HttpStatusCode.InternalServerError, null, additionalData);
    }

    public static ForbiddenException Forbidden(string message, object additionalData = null)
    {
        return new ForbiddenException(message, null, additionalData);
    }

#endregion
}