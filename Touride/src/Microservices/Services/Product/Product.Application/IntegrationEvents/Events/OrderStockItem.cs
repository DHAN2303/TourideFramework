namespace Product.Application.IntegrationEvents.Events
{
    public record OrderStockItem(Guid ProductId, int Units);
}