using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Touride.Framework.Api.Middlewares
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CustomHeadersToAddAndRemove _headers;


        public SecurityHeadersMiddleware(RequestDelegate next, CustomHeadersToAddAndRemove headers)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }
            _next = next;
            _headers = headers;
        }

        public async Task Invoke(HttpContext context)
        {
            foreach (var headerValuePair in _headers.HeadersToAdd)
            {
                context.Response.Headers[headerValuePair.Key] = headerValuePair.Value;
            }
            foreach (var header in _headers.HeadersToRemove)
            {
                context.Response.Headers.Remove(header);
            }

            await _next(context);
        }
    }

    public class CustomHeadersToAddAndRemove
    {
        public Dictionary<string, string> HeadersToAdd = new();
        public HashSet<string> HeadersToRemove = new();
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomSecurityHeaders(this IApplicationBuilder builder, Action<CustomHeadersToAddAndRemove> addHeadersAction)
        {
            var headers = new CustomHeadersToAddAndRemove();
            addHeadersAction?.Invoke(headers);

            builder.UseMiddleware<SecurityHeadersMiddleware>(headers);
            return builder;
        }
    }
}