using Microsoft.Extensions.DependencyInjection;
using Touride.Framework.Monitoring.AzureApplicationInsights;

namespace Touride.Framework.Monitoring
{
    public static class MonitoringExtensions
    {
        public static IServiceCollection AddMonitoringServices(this IServiceCollection services, MonitoringOptions monitoringOptions = null)
        {

            if (monitoringOptions?.AzureApplicationInsights?.IsEnabled ?? false)
            {
                services.AddAzureApplicationInsights(monitoringOptions.AzureApplicationInsights);
            }

            return services;
        }

    }
}
