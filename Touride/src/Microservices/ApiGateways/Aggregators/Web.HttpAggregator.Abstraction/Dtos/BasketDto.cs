namespace Web.HttpAggregator.Abstraction.Dtos
{
    public class BasketDto
    {
        public string BuyerId { get; set; } = "";
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
    }
}