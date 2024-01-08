namespace Basket.Abstraction.Dtos
{
    public class BasketDto
    {
        public BasketDto()
        {

        }

        public BasketDto(string customerId)
        {
            BuyerId = customerId;
        }
        public string BuyerId { get; set; } = "";
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();

    }
}