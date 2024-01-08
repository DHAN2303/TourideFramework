using Touride.Framework.Dapr.Events;

namespace Order.Application.IntegrationEvents
{
    public record OrderStatusChangedToSubmittedIntegrationEvent(
        Guid OrderId,
        string OrderStatus,
        string BuyerId,
        string BuyerEmail)
        : IntegrationEvent;
}