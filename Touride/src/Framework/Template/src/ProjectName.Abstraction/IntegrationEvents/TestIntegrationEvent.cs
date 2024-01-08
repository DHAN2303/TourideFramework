using Touride.Framework.Dapr.Events;

namespace ProjectName.Abstraction.IntegrationEvents
{
    public record TestIntegrationEvent(Guid Id, string Name) : IntegrationEvent;

}
