using Touride.Framework.Dapr.Events;

namespace Order.Application.IntegrationEvents
{
    public record OrderPaymentFailedIntegrationEvent(Guid OrderId) : IntegrationEvent;
}