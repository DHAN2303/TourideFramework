using Touride.Framework.Dapr.Events;

namespace Order.Application.IntegrationEvents
{
    public record OrderStatusChangedToValidatedIntegrationEvent(
        Guid OrderId,
        string OrderStatus,
        string Description,
        decimal Total,
        string BuyerId)
        : IntegrationEvent;
}