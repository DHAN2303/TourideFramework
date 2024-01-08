using Touride.Framework.Dapr.Events;

namespace Payment.Application.IntegrationEvents.Events
{
    public record OrderPaymentSucceededIntegrationEvent(Guid OrderId) : IntegrationEvent;
}