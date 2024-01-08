using Microsoft.AspNetCore.Builder;
using Serilog;
using Touride.Framework.Logging.Serilog.Middlewares;

namespace Touride.Framework.Logging.Serilog.Extensions
{
    public static class LoggingSerilogApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            return app;
        }
    }
}
