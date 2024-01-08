﻿using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Authentication;
using Touride.Framework.Abstractions.ExceptionHandling;

namespace Touride.Framework.Api.ExceptionHandling
{
    public class DefaultApiExceptionOptions
    {
        public void AddResponseDetails(HttpContext context, Exception exception, ErrorResult error)
        {
            var exceptionType = exception.GetType();
            HttpStatusCode status;
            if (exceptionType == typeof(AuthenticationException))
            {
                error.ErrorMessage = "Unauthenticated Access";
                status = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                error.ErrorMessage = "Unauthorized Access";
                status = HttpStatusCode.Forbidden;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                error.ErrorMessage = "A server error occurred.";
                status = HttpStatusCode.NotImplemented;
            }
            else
            {
                error.ErrorMessage = "Internal Server Error";
                status = HttpStatusCode.InternalServerError;
            }
            error.StatusCode = (int)status;
        }
    }
}
