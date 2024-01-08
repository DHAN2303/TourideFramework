using Touride.Framework.Dapr.Events;

namespace Product.Application.IntegrationEvents.Events
{
    public record OrderStockRejectedIntegrationEvent(
        Guid OrderId,
        List<ConfirmedOrderStockItem> OrderStockItems)
        : IntegrationEvent;
}