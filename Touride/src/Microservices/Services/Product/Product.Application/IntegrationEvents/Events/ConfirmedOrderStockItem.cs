namespace Product.Application.IntegrationEvents.Events
{
    public record ConfirmedOrderStockItem(Guid ProductId, bool HasStock);
}