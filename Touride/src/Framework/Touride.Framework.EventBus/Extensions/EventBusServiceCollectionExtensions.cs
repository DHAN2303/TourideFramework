using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Touride.Framework.EventBus;
using Touride.Framework.EventBus.Abstractions;

namespace Touride.Framework.EventBus.Extensions
{
    public static class EventBusServiceCollectionExtensions
    {
        public static void ConfigureEventBus(this IServiceCollection services, IConfiguration configuration)
        {

            var pubSubNames = configuration.GetSection("Touride.Framework:DaprEventBusSettings:PubSubNames").Get<ComponentModel[]>();

            if (pubSubNames != null)
            {
                foreach (var section in pubSubNames)
                {
                    services.AddScoped<IEventBus>(x =>
                                    new DaprEventBus(x.GetRequiredService<DaprClient>(),
                                    x.GetRequiredService<ILogger<DaprEventBus>>(),
                                    section.Name));

                }

            }

        }

    }
}