namespace Order.Application.IntegrationEvents
{
    public record OrderStockItem(Guid ProductId, int Units);
}