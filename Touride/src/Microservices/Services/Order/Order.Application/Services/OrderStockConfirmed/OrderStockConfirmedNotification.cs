using MediatR;

namespace Order.Application.Services.OrderStockConfirmed
{
    public class OrderStockConfirmedNotification : INotification
    {
        public OrderStockConfirmedNotification(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; set; }
    }
}
