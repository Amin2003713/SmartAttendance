using System;
using System.Net;

namespace Shifty.Common
{
    public class ShiftyException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public ApiResultStatusCode ApiStatusCode { get; set; }
        public object AdditionalData { get; set; }

        #region Constructors

        // Default constructor with ServerError status code
        public ShiftyException() : this(ApiResultStatusCode.ServerError)
        {
        }

        // Constructor with status code
        public ShiftyException(ApiResultStatusCode statusCode) : this(statusCode , null)
        {
        }

        // Constructor with message
        public ShiftyException(string message) : this(ApiResultStatusCode.ServerError , message)
        {
        }

        // Constructor with status code and message
        public ShiftyException(ApiResultStatusCode statusCode , string message) : this(statusCode , message , HttpStatusCode.InternalServerError)
        {
        }

        // Constructor with message and additional data
        public ShiftyException(string message , object additionalData) : this(ApiResultStatusCode.ServerError , message , additionalData)
        {
        }

        // Constructor with status code and additional data
        public ShiftyException(ApiResultStatusCode statusCode , object additionalData) : this(statusCode , null , additionalData)
        {
        }

        // Constructor with status code, message, and additional data
        public ShiftyException(ApiResultStatusCode statusCode , string message , object additionalData) : this(statusCode ,
            message ,
            HttpStatusCode.InternalServerError ,
            additionalData)
        {
        }

        // Constructor with status code, message, and HTTP status code
        public ShiftyException(ApiResultStatusCode statusCode , string message , HttpStatusCode httpStatusCode) : this(statusCode ,
            message ,
            httpStatusCode ,
            null)
        {
        }

        // Constructor with status code, message, HTTP status code, and additional data
        public ShiftyException(ApiResultStatusCode statusCode , string message , HttpStatusCode httpStatusCode , object additionalData) : this(statusCode ,
            message ,
            httpStatusCode ,
            null ,
            additionalData)
        {
        }

        // Constructor with message and exception
        public ShiftyException(string message , Exception exception) : this(ApiResultStatusCode.ServerError , message , exception)
        {
        }

        // Constructor with message, exception, and additional data
        public ShiftyException(string message , Exception exception , object additionalData) : this(ApiResultStatusCode.ServerError ,
            message ,
            exception ,
            additionalData)
        {
        }

        // Constructor with status code, message, and exception
        public ShiftyException(ApiResultStatusCode statusCode , string message , Exception exception) : this(statusCode ,
            message ,
            HttpStatusCode.InternalServerError ,
            exception)
        {
        }

        // Constructor with status code, message, exception, and additional data
        public ShiftyException(ApiResultStatusCode statusCode , string message , Exception exception , object additionalData) : this(statusCode ,
            message ,
            HttpStatusCode.InternalServerError ,
            exception ,
            additionalData)
        {
        }

        // Constructor with status code, message, HTTP status code, and exception
        public ShiftyException(ApiResultStatusCode statusCode , string message , HttpStatusCode httpStatusCode , Exception exception) : this(statusCode ,
            message ,
            httpStatusCode ,
            exception ,
            null)
        {
        }

        // Full constructor with all parameters
        public ShiftyException(ApiResultStatusCode statusCode , string message , HttpStatusCode httpStatusCode , Exception exception , object additionalData)
            : base(message , exception)
        {
            ApiStatusCode  = statusCode;
            HttpStatusCode = httpStatusCode;
            AdditionalData = additionalData;
        }

        #endregion
    }
}