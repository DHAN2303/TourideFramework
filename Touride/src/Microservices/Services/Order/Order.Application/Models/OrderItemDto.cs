using Order.Application.Actors;

namespace Order.Application.Models
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Units { get; set; }
        public string PictureFileName { get; set; }

        public OrderItemDto()
        {
            OrderId = Guid.Empty;
            ProductName = string.Empty;
            PictureFileName = string.Empty;
        }

        public OrderItemDto(OrderItemState state)
        {
            ProductId = state.ProductId;
            ProductName = state.ProductName;
            UnitPrice = state.UnitPrice;
            Units = state.Units;
            PictureFileName = state.PictureFileName;
        }
    }
}