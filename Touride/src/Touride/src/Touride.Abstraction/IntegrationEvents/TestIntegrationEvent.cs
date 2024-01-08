using Touride.Framework.Dapr.Events;

namespace Touride.Abstraction.IntegrationEvents
{
    public record TestIntegrationEvent(Guid Id, string Name) : IntegrationEvent;

}
