using Order.Application.Models;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.MediatR.Configuration.Commands;

namespace Order.Application.Services.OrderStatusChanged
{
    public class OrderStatusChangedCommand : ICommand<Result<OrderDto>>
    {
        public OrderStatusChangedCommand(
            Guid orderId,
            string orderStatus,
            string buyerId,
            string buyerEmail
            )
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
            BuyerId = buyerId;
            BuyerEmail = buyerEmail;
        }

        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string OrderStatus { get; set; }
        public string BuyerId { get; set; }
        public string BuyerEmail { get; set; }

    }
}
