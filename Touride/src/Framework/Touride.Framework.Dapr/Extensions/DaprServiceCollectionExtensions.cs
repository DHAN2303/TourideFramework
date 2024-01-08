using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Touride.Framework.Dapr.Abstractions;

namespace Touride.Framework.Dapr.Extensions
{
    public static class DaprServiceCollectionExtensions
    {
        public static void ConfigureEventBus(this IServiceCollection services, IConfiguration configuration)
        {

            var pubSubName = configuration.GetValue<string>("Touride.Framework:DaprSettings:PubSubName");

            if (pubSubName != null)
            {
                services.AddScoped<IEventBus>(x =>
                                new DaprEventBus(x.GetRequiredService<DaprClient>(),
                                x.GetRequiredService<ILogger<DaprEventBus>>(),
                                pubSubName));
            }

        }

        public static void ConfigureStateStore(this IServiceCollection services)
        {
            services.AddScoped<IDaprStateStore>(sp => new DaprStateStore(sp.GetRequiredService<ILogger<DaprStateStore>>()));
        }

    }
}