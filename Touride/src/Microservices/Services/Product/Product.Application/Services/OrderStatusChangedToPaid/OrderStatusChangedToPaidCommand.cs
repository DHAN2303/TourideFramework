using Product.Application.IntegrationEvents.Events;
using Product.Domain.AggregatesModel.CatalogItemAggregate;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.MediatR.Configuration.Commands;

namespace Product.Application.Services.OrderStatusChangedToPaid
{
    public class OrderStatusChangedToPaidCommand : ICommand<Result<List<CatalogItem>>>
    {
        public OrderStatusChangedToPaidCommand(Guid orderId, IEnumerable<OrderStockItem> orderStockItems)
        {
            OrderId = orderId;
            OrderStockItems = orderStockItems;
        }

        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public IEnumerable<OrderStockItem> OrderStockItems { get; set; }
    }
}
