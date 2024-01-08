using Touride.Framework.Dapr.Events;

namespace Payment.Application.IntegrationEvents.Events
{
    public record OrderStatusChangedToValidatedIntegrationEvent(
        Guid OrderId,
        decimal Total)
        : IntegrationEvent;
}