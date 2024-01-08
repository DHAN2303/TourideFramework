using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Serilog.Context;
using System.Diagnostics;
using System.Globalization;

namespace Touride.Framework.Api.Middlewares
{
    public class RequestResponseMiddleware
    {
        private const string CorrelationId = "Correlation-Id";
        private const string CorrelationSeq = "Correlation-Seq";
        private const string Authorization = "Authorization";
        private const string Culture = "Culture";
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public RequestResponseMiddleware(RequestDelegate next,
        ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory.CreateLogger<RequestResponseMiddleware>();
        }
        public async Task Invoke(HttpContext httpContext)
        {
            string correlationId = null;
            string correlationSeq = null;
            string tokenValue = null;
            string culture = null;

            #region Culture
            if (httpContext.Request.Headers.TryGetValue(Culture, out StringValues cultures))
                culture = cultures.FirstOrDefault();
            else
            {
                culture = "tr-TR";
                httpContext.Request.Headers.Add(Culture, culture);
            }

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            #endregion

            #region CorrelationId
            if (httpContext.Request.Headers.TryGetValue(CorrelationId, out StringValues correlationIds))
                correlationId = correlationIds.FirstOrDefault();
            else
            {
                correlationId = Guid.NewGuid().ToString();
                httpContext.Request.Headers.Add(CorrelationId, correlationId);
            }
            Debug.WriteLine($"CorrelationId(Middleware): {correlationId}");
            #endregion

            #region CorrelationSeq
            if (httpContext.Request.Headers.TryGetValue(CorrelationSeq, out StringValues correlationSeqs))
                correlationSeq = correlationSeqs.FirstOrDefault();
            else
            {
                correlationSeq = "1";
                httpContext.Request.Headers.Add(CorrelationSeq, correlationSeq);
            }
            #endregion


            httpContext.Response.OnStarting(() =>
            {
                if (!httpContext.Response.Headers.TryGetValue(CorrelationId, out correlationIds))
                    httpContext.Response.Headers.Add(CorrelationId, correlationId);
                if (!httpContext.Response.Headers.TryGetValue(CorrelationSeq, out correlationSeqs))
                    httpContext.Response.Headers.Add(CorrelationSeq, correlationSeq);
                if (!httpContext.Response.Headers.TryGetValue(Authorization, out StringValues token))
                    httpContext.Response.Headers.Add(Authorization, token);
                if (!httpContext.Response.Headers.TryGetValue(Culture, out StringValues culture))
                    httpContext.Response.Headers.Add(Culture, culture);
                return Task.CompletedTask;
            });
            LogContext.PushProperty(CorrelationId, correlationId);
            LogContext.PushProperty(CorrelationSeq, correlationSeq);
            await _next.Invoke(httpContext);
        }
    }
}