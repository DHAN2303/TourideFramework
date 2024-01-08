using Basket.Abstraction.Dtos;
using Basket.Application.IntegrationEvents.Events;
using Dapr.Client;
using MediatR;
using Touride.Framework.Dapr.Abstractions;

namespace Basket.Application.Services.BasketServices.CheckoutBasket
{
    public class BasketCheckoutNotificationHandler : INotificationHandler<BasketCheckoutNotification>
    {
        private readonly IDaprStateStore _daprStateStore;
        private readonly IEventBus _eventBus;
        public BasketCheckoutNotificationHandler(IDaprStateStore daprStateStore, IEventBus eventBus)
        {
            _daprStateStore = daprStateStore;
            _eventBus = eventBus;
        }
        private const string DAPR_PUBSUB_NAME = "touride-pubsub";
        public async Task Handle(BasketCheckoutNotification notification, CancellationToken cancellationToken)
        {
            var basket = await _daprStateStore.GetStateAsync<BasketDto>(DAPR_PUBSUB_NAME, notification.UserId);

            var eventRequestId = Guid.TryParse(notification.RequestId, out Guid parsedRequestId)
                ? parsedRequestId : Guid.NewGuid();

            var eventMessage = new UserCheckoutAcceptedIntegrationEvent(
                notification.UserId,
                notification.UserEmail,
                notification.City,
                notification.Street,
                notification.State,
                notification.Country,
                notification.CardNumber,
                notification.CardHolderName,
                notification.CardExpiration,
                notification.CardSecurityCode,
                eventRequestId,
                basket);

            await _eventBus.PublishAsync(eventMessage);
        }

    }
}