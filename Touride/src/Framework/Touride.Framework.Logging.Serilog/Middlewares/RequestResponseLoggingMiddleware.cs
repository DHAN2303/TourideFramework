using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog;
using Serilog.Context;
using System.Text;

namespace Touride.Framework.Logging.Serilog.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private const string CorrelationId = "Correlation-Id";
        private const string CorrelationSeq = "Correlation-Seq";
        private const string Culture = "Culture";
        private readonly RequestDelegate _next;
        private readonly IDiagnosticContext _diagnosticContext;

        public RequestResponseLoggingMiddleware(RequestDelegate next, IDiagnosticContext diagnosticContext)
        {
            _next = next;
            _diagnosticContext = diagnosticContext;
        }

        public async Task Invoke(HttpContext context)
        {
            _diagnosticContext.Set("Host", context.Request.Host);
            _diagnosticContext.Set("Protocol", context.Request.Protocol);
            _diagnosticContext.Set("Scheme", context.Request.Scheme);

            if (context.Request.Headers.TryGetValue(CorrelationId, out StringValues correlationIds))
            {
                var correlationId = correlationIds.FirstOrDefault();
                _diagnosticContext.Set(CorrelationId, correlationId);
                LogContext.PushProperty(CorrelationId, correlationId);
            }
            if (context.Request.Headers.TryGetValue(CorrelationSeq, out StringValues correlationSeqs))
            {
                var correlationSeq = correlationSeqs.FirstOrDefault();
                _diagnosticContext.Set(CorrelationSeq, correlationSeq);
                LogContext.PushProperty(CorrelationSeq, correlationSeq);
            }
            if (context.Request.Headers.TryGetValue(Culture, out StringValues culture))
            {
                var _culture = culture.FirstOrDefault();
                _diagnosticContext.Set(Culture, _culture);
                LogContext.PushProperty(Culture, _culture);
            }
            if (context.Request.QueryString.HasValue)
            {
                _diagnosticContext.Set("QueryString", context.Request.QueryString.Value);
            }
            string requestBodyPayload = await ReadRequestBody(context.Request);
            _diagnosticContext.Set("RequestBody", requestBodyPayload);

            var endpoint = context.GetEndpoint();
            if (endpoint is object)
            {
                _diagnosticContext.Set("EndpointName", endpoint.DisplayName);
            }

            using (var responseBody = new MemoryStream())
            {
                var originalResponseBodyStream = context.Response.Body;
                context.Response.Body = responseBody;
                await _next(context);
                string responseBodyPayload = await ReadResponseBody(context.Response);
                _diagnosticContext.Set("ResponseBody", responseBodyPayload);
                await responseBody.CopyToAsync(originalResponseBodyStream);
            }
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            HttpRequestRewindExtensions.EnableBuffering(request);

            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            string requestBody = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            return $"{requestBody}";
        }

        private static async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return $"{responseBody}";
        }
    }
}
