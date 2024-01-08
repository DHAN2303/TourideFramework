using Order.Application.Models;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.MediatR.Configuration.Commands;

namespace Order.Application.Services.OrderUpdateStatus
{
    public class UpdateOrderStatusCommand : ICommand<Result<OrderDto>>
    {
        public UpdateOrderStatusCommand(Guid id, string description, string orderStatus)
        {
            Id = id;
            Description = description;
            OrderStatus = orderStatus;
        }

        public Guid Id { get; set; }
        public string OrderStatus { get; set; }
        public string Description { get; set; }
    }
}
