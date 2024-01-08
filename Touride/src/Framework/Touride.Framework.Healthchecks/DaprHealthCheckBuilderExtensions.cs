using Microsoft.Extensions.DependencyInjection;

namespace Touride.Framework.Healthchecks
{
    public static class DaprHealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddDapr(this IHealthChecksBuilder builder) =>
            builder.AddCheck<DaprHealthCheck>("dapr");
    }
}