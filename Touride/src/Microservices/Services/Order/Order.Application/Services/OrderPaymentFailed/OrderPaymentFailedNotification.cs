using MediatR;

namespace Order.Application.Services.OrderPaymentFailed
{
    public class OrderPaymentFailedNotification : INotification
    {
        public OrderPaymentFailedNotification(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; set; }
    }
}
