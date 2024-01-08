namespace Order.Application.Actors
{
    public class OrderItemState
    {
        // int Id, TODO
        //int OrderId, TODO
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; } = -1;
        public int Units { get; set; } = -1;
        public string PictureFileName { get; set; } = string.Empty;
    }
}