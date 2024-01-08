using Order.Domain.AggregatesModel.AddressAggregate;
using Order.Domain.AggregatesModel.OrderItemAggregate;
using Touride.Framework.Abstractions.Data.AuditProperties;
using Touride.Framework.Abstractions.Data.Entities;

namespace Order.Domain.AggregatesModel.OrderAggregate
{
    public class Order : IHasId, IHasFullAudit
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
        public Guid Id { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderStatus { get; set; }
        public string? Description { get; set; }
        public Address? Address { get; set; }
        public string? BuyerId { get; set; }
        public string? BuyerEmail { get; set; }
        public List<OrderItem>? OrderItems { get; set; }

    }
}