using Dapr.Actors;
using Dapr.Actors.Client;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Actors;
using Order.Application.Services.OrderStockConfirmed;

namespace Order.Application.Services.OrderStockRejected
{
    public class OrderStockRejectedNotificationHandler : INotificationHandler<OrderStockRejectedNotification>
    {
        private readonly IActorProxyFactory _actorProxyFactory;
        private readonly ILogger<OrderStockConfirmedNotificationHandler> _logger;
        public OrderStockRejectedNotificationHandler(IActorProxyFactory actorProxyFactory, ILogger<OrderStockConfirmedNotificationHandler> logger)
        {
            _actorProxyFactory = actorProxyFactory;
            _logger = logger;
        }
        public async Task Handle(OrderStockRejectedNotification notification, CancellationToken cancellationToken)
        {
            var outOfStockItems = notification.OrderStockItems
                .FindAll(c => !c.HasStock)
                .Select(c => c.ProductId)
                .ToList();

            var actorId = new ActorId(notification.OrderId.ToString());

            var process = _actorProxyFactory.CreateActorProxy<IOrderingProcessActor>(
                actorId,
                nameof(OrderingProcessActor));

            await process.StockRejectedSimulatedWorkDoneAsync(outOfStockItems);
        }
    }
}
