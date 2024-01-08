using Touride.Framework.Dapr.Events;

namespace Order.Application.IntegrationEvents
{
    public record OrderStatusChangedToPaidIntegrationEvent(
        Guid OrderId,
        string OrderStatus,
        string Description,
        IEnumerable<OrderStockItem> OrderStockItems,
        string BuyerId)
        : IntegrationEvent;
}