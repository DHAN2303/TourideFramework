using MediatR;
using Order.Application.Models;
using Touride.Framework.Abstractions.Application.Models;

namespace Order.Application.Services.Queryies.GetAllOrder
{
    public class GetAllOrderQuery : IRequest<Result<List<OrderDto>>>
    {
    }
}
