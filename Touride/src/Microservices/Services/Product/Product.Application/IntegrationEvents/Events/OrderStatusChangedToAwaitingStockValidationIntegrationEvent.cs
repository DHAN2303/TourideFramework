using Touride.Framework.Dapr.Events;

namespace Product.Application.IntegrationEvents.Events
{
    public record OrderStatusChangedToAwaitingStockValidationIntegrationEvent(
        Guid OrderId,
        IEnumerable<OrderStockItem> OrderStockItems)
        : IntegrationEvent;
}