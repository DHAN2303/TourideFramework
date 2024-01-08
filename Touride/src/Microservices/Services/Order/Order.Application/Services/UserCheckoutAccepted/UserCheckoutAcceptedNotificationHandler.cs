using Dapr.Actors;
using Dapr.Actors.Client;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Actors;

namespace Order.Application.Services.UserCheckoutAccepted
{
    public class OrderStatusChangedNotificationHandler : INotificationHandler<UserCheckoutAcceptedNotification>
    {
        private readonly IActorProxyFactory _actorProxyFactory;
        private readonly ILogger<OrderStatusChangedNotificationHandler> _logger;

        public OrderStatusChangedNotificationHandler(
            IActorProxyFactory actorProxyFactory,
            ILogger<OrderStatusChangedNotificationHandler> logger)
        {
            _actorProxyFactory = actorProxyFactory;
            _logger = logger;
        }
        public async Task Handle(UserCheckoutAcceptedNotification notification, CancellationToken cancellationToken)
        {
            if (notification.RequestId != Guid.Empty)
            {
                var actorId = new ActorId(notification.RequestId.ToString());

                var orderingProcess = _actorProxyFactory.CreateActorProxy<IOrderingProcessActor>(
                actorId,
                    nameof(OrderingProcessActor));

                await orderingProcess.SubmitAsync(
                notification.UserId, notification.UserEmail, notification.Street, notification.City,
                notification.State, notification.Country, notification.CustomerBasket);

            }
            else
            {
                _logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", notification);
            }
        }
    }
}
