using Touride.Framework.Dapr.Events;

namespace Touride.Framework.Dapr.Abstractions
{
    public interface IEventBus
    {
        Task PublishAsync(IntegrationEvent integrationEvent);
    }
}