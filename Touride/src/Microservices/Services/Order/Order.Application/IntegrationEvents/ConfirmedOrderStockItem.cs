namespace Order.Application.IntegrationEvents
{
    public record ConfirmedOrderStockItem(Guid ProductId, bool HasStock);
}