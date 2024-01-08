using Touride.Framework.Dapr.Events;

namespace Product.Application.IntegrationEvents.Events
{
    public record OrderStockConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent;
}