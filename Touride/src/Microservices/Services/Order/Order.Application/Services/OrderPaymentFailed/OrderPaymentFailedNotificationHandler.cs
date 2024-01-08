using Dapr.Actors;
using Dapr.Actors.Client;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Actors;
using Order.Application.Services.OrderStockConfirmed;
using Touride.Framework.Client.Abstractions;

namespace Order.Application.Services.OrderPaymentFailed
{
    public class OrderPaymentFailedNotificationHandler : INotificationHandler<OrderPaymentFailedNotification>
    {
        private readonly IActorProxyFactory _actorProxyFactory;
        private readonly ILogger<OrderStockConfirmedNotificationHandler> _logger;
        private readonly IHttpClientProvider _httpClient;

        public OrderPaymentFailedNotificationHandler(IActorProxyFactory actorProxyFactory, ILogger<OrderStockConfirmedNotificationHandler> logger, IHttpClientProvider httpClient)
        {
            _actorProxyFactory = actorProxyFactory;
            _logger = logger;
            _httpClient = httpClient;
        }
        public async Task Handle(OrderPaymentFailedNotification notification, CancellationToken cancellationToken)
        {
            var actorId = new ActorId(notification.OrderId.ToString());
            var process = _actorProxyFactory.CreateActorProxy<IOrderingProcessActor>(
                actorId,
                nameof(OrderingProcessActor));

            await process.PaymentFailedSimulatedWorkDoneAsync();
        }
    }
}
