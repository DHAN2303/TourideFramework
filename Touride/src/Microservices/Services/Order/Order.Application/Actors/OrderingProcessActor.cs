using Dapr.Actors.Runtime;
using Microsoft.Extensions.Logging;
using Order.Application.IntegrationEvents;
using Touride.Framework.Dapr.Abstractions;

namespace Order.Application.Actors
{
    public class OrderingProcessActor : Actor, IOrderingProcessActor
    {
        private const string OrderDetailsStateName = "OrderDetails";
        private const string OrderStatusStateName = "OrderStatus";

        private readonly IEventBus _eventBus;

        private int? _preMethodOrderStatusId;

        public OrderingProcessActor(
            ActorHost host,
            IEventBus eventBus)
            : base(host)
        {
            _eventBus = eventBus;
        }

        private Guid OrderId => Guid.Parse(Id.GetId());

        public async Task SubmitAsync(
            string buyerId,
            string buyerEmail,
            string street,
            string city,
            string state,
            string country,
            CustomerBasket basket)
        {
            var orderState = new OrderState
            {
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.Submitted,
                Description = "Submitted",
                Address = new OrderAddressState
                {
                    Street = street,
                    City = city,
                    State = state,
                    Country = country
                },
                BuyerId = buyerId,
                BuyerEmail = buyerEmail,
                OrderItems = basket.Items
                    .Select(item => new OrderItemState
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        UnitPrice = item.UnitPrice,
                        Units = item.Quantity,
                        PictureFileName = item.PictureFileName
                    })
                    .ToList()
            };

            await StateManager.SetStateAsync(OrderDetailsStateName, orderState);
            await StateManager.SetStateAsync(OrderStatusStateName, OrderStatus.Submitted);

            var order = await StateManager.GetStateAsync<OrderState>(OrderDetailsStateName);

            await _eventBus.PublishAsync(new OrderStatusChangedToSubmittedIntegrationEvent(
                OrderId,
                OrderStatus.Submitted.Name,
                buyerId,
                buyerEmail));
        }


        public async Task<bool> CancelAsync()
        {
            var orderStatus = await StateManager.TryGetStateAsync<OrderStatus>(OrderStatusStateName);
            if (!orderStatus.HasValue)
            {
                Logger.LogWarning("Order with Id: {OrderId} cannot be cancelled because it doesn't exist",
                    OrderId);

                return false;
            }

            if (orderStatus.Value.Id == OrderStatus.Paid.Id || orderStatus.Value.Id == OrderStatus.Shipped.Id)
            {
                Logger.LogWarning("Order with Id: {OrderId} cannot be cancelled because it's in status {Status}",
                    OrderId, orderStatus.Value.Name);

                return false;
            }

            await StateManager.SetStateAsync(OrderStatusStateName, OrderStatus.Cancelled);

            var order = await StateManager.GetStateAsync<OrderState>(OrderDetailsStateName);

            await _eventBus.PublishAsync(new OrderStatusChangedToCancelledIntegrationEvent(
                OrderId,
                OrderStatus.Cancelled.Name,
                $"The order was cancelled by buyer.",
                order.BuyerId));

            return true;
        }

        public async Task<bool> ShipAsync()
        {
            var statusChanged = await TryUpdateOrderStatusAsync(OrderStatus.Paid, OrderStatus.Shipped);
            if (statusChanged)
            {
                var order = await StateManager.GetStateAsync<OrderState>(OrderDetailsStateName);

                await _eventBus.PublishAsync(new OrderStatusChangedToShippedIntegrationEvent(
                    OrderId,
                    OrderStatus.Shipped.Name,
                    "The order was shipped.",
                    order.BuyerId));

                return true;
            }

            return false;
        }

        public async Task<OrderState> GetOrderDetails()
        {
            return await StateManager.GetStateAsync<OrderState>(OrderDetailsStateName);
        }

        public async Task OrderStatusChangedToSubmittedAsync()
        {
            var statusChanged = await TryUpdateOrderStatusAsync(OrderStatus.Submitted, OrderStatus.AwaitingStockValidation);

            if (statusChanged)
            {
                var order = await StateManager.GetStateAsync<OrderState>(OrderDetailsStateName);

                await _eventBus.PublishAsync(new OrderStatusChangedToAwaitingStockValidationIntegrationEvent(
                    OrderId,
                    OrderStatus.AwaitingStockValidation.Name,
                    "Grace period elapsed; waiting for stock validation.",
                    order.OrderItems
                        .Select(orderItem => new OrderStockItem(orderItem.ProductId, orderItem.Units)),
                    order.BuyerId));
            }
        }

        public async Task StockConfirmedSimulatedWorkDoneAsync()
        {
            var order = await StateManager.GetStateAsync<OrderState>(OrderDetailsStateName);

            await _eventBus.PublishAsync(new OrderStatusChangedToValidatedIntegrationEvent(
                OrderId,
                OrderStatus.Validated.Name,
                "All the items were confirmed with available stock.",
                order.GetTotal(),
                order.BuyerId));
        }

        public async Task StockRejectedSimulatedWorkDoneAsync(List<Guid> rejectedProductIds)
        {
            var order = await StateManager.GetStateAsync<OrderState>(OrderDetailsStateName);

            var rejectedProductNames = order.OrderItems
                .Where(orderItem => rejectedProductIds.Contains(orderItem.ProductId))
                .Select(orderItem => orderItem.ProductName);

            var rejectedDescription = string.Join(", ", rejectedProductNames);

            await _eventBus.PublishAsync(new OrderStatusChangedToCancelledIntegrationEvent(
                OrderId,
                OrderStatus.Cancelled.Name,
                $"The following product items don't have stock: ({rejectedDescription}).",
                order.BuyerId));
        }

        public async Task PaymentSucceededSimulatedWorkDoneAsync()
        {
            var order = await StateManager.GetStateAsync<OrderState>(OrderDetailsStateName);

            await _eventBus.PublishAsync(new OrderStatusChangedToPaidIntegrationEvent(
                OrderId,
                OrderStatus.Paid.Name,
                "The payment was performed at a simulated \"American Bank checking bank account ending on XX35071\"",
                order.OrderItems
                    .Select(orderItem => new OrderStockItem(orderItem.ProductId, orderItem.Units)),
                order.BuyerId));
        }

        public async Task PaymentFailedSimulatedWorkDoneAsync()
        {
            var order = await StateManager.GetStateAsync<OrderState>(OrderDetailsStateName);

            await _eventBus.PublishAsync(new OrderStatusChangedToCancelledIntegrationEvent(
                OrderId,
                OrderStatus.Cancelled.Name,
                "The order was cancelled because payment failed.",
                order.BuyerId));
        }

        protected override async Task OnPreActorMethodAsync(ActorMethodContext actorMethodContext)
        {
            var orderStatus = await StateManager.TryGetStateAsync<OrderStatus>(OrderStatusStateName);

            _preMethodOrderStatusId = orderStatus.HasValue ? orderStatus.Value.Id : (int?)null;
        }

        protected override async Task OnPostActorMethodAsync(ActorMethodContext actorMethodContext)
        {
            var postMethodOrderStatus = await StateManager.GetStateAsync<OrderStatus>(OrderStatusStateName);

            if (_preMethodOrderStatusId != postMethodOrderStatus.Id)
            {
                Logger.LogInformation("Order with Id: {OrderId} has been updated to status {Status}",
                    OrderId, postMethodOrderStatus.Name);
            }
        }

        private async Task<bool> TryUpdateOrderStatusAsync(OrderStatus expectedOrderStatus, OrderStatus newOrderStatus)
        {
            var orderStatus = await StateManager.TryGetStateAsync<OrderStatus>(OrderStatusStateName);
            if (!orderStatus.HasValue)
            {
                Logger.LogWarning("Order with Id: {OrderId} cannot be updated because it doesn't exist",
                    OrderId);

                return false;
            }

            if (orderStatus.Value.Id != expectedOrderStatus.Id)
            {
                Logger.LogWarning("Order with Id: {OrderId} is in status {Status} instead of expected status {ExpectedStatus}",
                    OrderId, orderStatus.Value.Name, expectedOrderStatus.Name);

                return false;
            }

            await StateManager.SetStateAsync(OrderStatusStateName, newOrderStatus);

            return true;
        }
    }
}