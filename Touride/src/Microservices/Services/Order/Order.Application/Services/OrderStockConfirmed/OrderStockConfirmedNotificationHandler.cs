using Dapr.Actors;
using Dapr.Actors.Client;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Actors;

namespace Order.Application.Services.OrderStockConfirmed
{
    public class OrderStockConfirmedNotificationHandler : INotificationHandler<OrderStockConfirmedNotification>
    {
        private readonly IActorProxyFactory _actorProxyFactory;
        private readonly ILogger<OrderStockConfirmedNotificationHandler> _logger;

        public OrderStockConfirmedNotificationHandler(IActorProxyFactory actorProxyFactory, ILogger<OrderStockConfirmedNotificationHandler> logger)
        {
            _actorProxyFactory = actorProxyFactory;
            _logger = logger;
        }
        public async Task Handle(OrderStockConfirmedNotification notification, CancellationToken cancellationToken)
        {
            var actorId = new ActorId(notification.OrderId.ToString());
            var process = _actorProxyFactory.CreateActorProxy<IOrderingProcessActor>(
                actorId,
                nameof(OrderingProcessActor));

            await process.StockConfirmedSimulatedWorkDoneAsync();
        }
    }
}
