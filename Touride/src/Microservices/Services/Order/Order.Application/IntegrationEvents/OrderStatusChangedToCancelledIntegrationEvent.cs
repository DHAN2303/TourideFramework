using Touride.Framework.Dapr.Events;

namespace Order.Application.IntegrationEvents
{
    public record OrderStatusChangedToCancelledIntegrationEvent(
        Guid OrderId,
        string OrderStatus,
        string Description,
        string BuyerId)
        : IntegrationEvent;
}