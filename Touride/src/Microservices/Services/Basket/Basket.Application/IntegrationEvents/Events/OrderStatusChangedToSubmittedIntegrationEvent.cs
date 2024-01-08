using Touride.Framework.Dapr.Events;

namespace Basket.Application.IntegrationEvents.Events
{
    public record OrderStatusChangedToSubmittedIntegrationEvent(
        Guid OrderId,
        string OrderStatus,
        string BuyerId)
        : IntegrationEvent;
}