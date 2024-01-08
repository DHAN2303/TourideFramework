using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Touride.Framework.Abstractions.Client;

namespace Touride.Framework.Api.Providers
{
    public class HttpContextProvider : IHttpContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? CorrelationId => _httpContextAccessor?.HttpContext?.Request?.Headers["Correlation-Id"].ToString();
        public string? CorrelationSeq => _httpContextAccessor?.HttpContext?.Request?.Headers["Correlation-Seq"].ToString();
        public string? Token => _httpContextAccessor?.HttpContext?.Request?.Headers["Authorization"].ToString();
        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    }
}