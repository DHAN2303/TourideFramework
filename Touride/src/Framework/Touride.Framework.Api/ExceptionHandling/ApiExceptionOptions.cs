using Microsoft.AspNetCore.Http;
using Touride.Framework.Abstractions.ExceptionHandling;

namespace Touride.Framework.Api.ExceptionHandling
{
    public class ApiExceptionOptions
    {
        public Action<HttpContext, Exception, ErrorResult> AddResponseDetails { get; set; }
        public Func<Exception, Microsoft.Extensions.Logging.LogLevel> DetermineLogLevel { get; set; }
    }
}
