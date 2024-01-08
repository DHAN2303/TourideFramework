using Touride.Framework.Dapr.Events;

namespace Product.Application.IntegrationEvents.Events
{
    public record OrderStatusChangedToPaidIntegrationEvent(
        Guid OrderId,
        IEnumerable<OrderStockItem> OrderStockItems)
        : IntegrationEvent;
}