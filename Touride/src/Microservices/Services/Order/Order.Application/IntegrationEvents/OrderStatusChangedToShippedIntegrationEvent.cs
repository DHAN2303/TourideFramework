using Touride.Framework.Dapr.Events;

namespace Order.Application.IntegrationEvents
{
    public record OrderStatusChangedToShippedIntegrationEvent(
        Guid OrderId,
        string OrderStatus,
        string Description,
        string BuyerId)
        : IntegrationEvent;
}