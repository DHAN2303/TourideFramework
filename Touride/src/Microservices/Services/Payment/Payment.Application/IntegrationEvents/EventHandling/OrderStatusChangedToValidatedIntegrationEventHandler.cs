using Microsoft.Extensions.Logging;
using Payment.Application.IntegrationEvents.Events;
using Touride.Framework.Dapr.Abstractions;
using Touride.Framework.Dapr.Events;

namespace Payment.Application.IntegrationEvents.EventHandling
{
    public class OrderStatusChangedToValidatedIntegrationEventHandler :
        IIntegrationEventHandler<OrderStatusChangedToValidatedIntegrationEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger _logger;

        public OrderStatusChangedToValidatedIntegrationEventHandler(
            IEventBus eventBus,
            ILogger<OrderStatusChangedToValidatedIntegrationEventHandler> logger)
        {
            _eventBus = eventBus;
            _logger = logger;
        }

        public async Task Handle(OrderStatusChangedToValidatedIntegrationEvent @event)
        {
            IntegrationEvent orderPaymentIntegrationEvent;

            //Ödeme işlemlerinin yapılacağı kısım

            await Task.Delay(3000); // ödemenin banka tarafından onayı bekleniyor 😉

            if (true)//Ödeme gerçekleştiyse
            {
                orderPaymentIntegrationEvent = new OrderPaymentSucceededIntegrationEvent(@event.OrderId);
            }
            else
            {
                _logger.LogWarning(
                    "Payment for ${Total} rejected for order {OrderId} because of service configuration",
                    @event.Total,
                    @event.OrderId);

                orderPaymentIntegrationEvent = new OrderPaymentFailedIntegrationEvent(@event.OrderId);
            }

            await _eventBus.PublishAsync(orderPaymentIntegrationEvent);
        }
    }
}