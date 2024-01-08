using Touride.Framework.EventBus.Events;

namespace Touride.Framework.EventBus.Abstractions
{
    public interface IEventBus
    {
        Task PublishAsync(IntegrationEvent integrationEvent);
    }
}