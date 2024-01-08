using Touride.Framework.Dapr.Events;

namespace Order.Application.IntegrationEvents
{
    public record OrderPaymentSucceededIntegrationEvent(Guid OrderId) : IntegrationEvent;
}