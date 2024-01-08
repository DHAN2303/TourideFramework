using MediatR;

namespace Order.Application.Services.OrderPaymentSucceeded
{
    public class OrderPaymentSucceededNotification : INotification
    {
        public OrderPaymentSucceededNotification(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; set; }
    }
}
