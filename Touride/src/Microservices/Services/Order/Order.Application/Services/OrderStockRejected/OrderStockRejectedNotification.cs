using MediatR;
using Order.Application.IntegrationEvents;

namespace Order.Application.Services.OrderStockRejected
{
    public class OrderStockRejectedNotification : INotification
    {
        public OrderStockRejectedNotification(Guid orderId, List<ConfirmedOrderStockItem> orderStockItems)
        {
            OrderId = orderId;
            OrderStockItems = orderStockItems;
        }

        public Guid OrderId { get; set; }
        public List<ConfirmedOrderStockItem> OrderStockItems { get; set; }
    }
}
