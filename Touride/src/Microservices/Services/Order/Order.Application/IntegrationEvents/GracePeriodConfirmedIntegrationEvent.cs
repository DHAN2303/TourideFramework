using Touride.Framework.Dapr.Events;

namespace Order.Application.IntegrationEvents
{
    public record GracePeriodConfirmedIntegrationEvent(int OrderId) : IntegrationEvent;
}