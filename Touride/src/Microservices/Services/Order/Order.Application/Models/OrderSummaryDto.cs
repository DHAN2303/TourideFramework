namespace Order.Application.Models
{
    public record OrderSummaryDto(
        Guid Id,
        int OrderNumber,
        DateTime OrderDate,
        string OrderStatus,
        decimal Total);
}