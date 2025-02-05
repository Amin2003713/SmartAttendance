﻿using System;
using System.Net;

namespace Shifty.Common.Exceptions;

/// <summary>
/// Represents an HTTP 403 Forbidden error.
/// </summary>
public class ForbiddenException : ShiftyException
{
    
    public ForbiddenException(string message , Exception innerException , object additionalData = null) : base(message ,
        HttpStatusCode.Forbidden ,
        innerException ,
        additionalData)
    {
    }
}

