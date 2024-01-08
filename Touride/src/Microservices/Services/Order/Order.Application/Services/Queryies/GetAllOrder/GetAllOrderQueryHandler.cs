using MediatR;
using Order.Application.Models;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.Data.Abstractions;

namespace Order.Application.Services.Queryies.GetAllOrder
{
    public class GetAllOrderQueryHandler : IRequestHandler<GetAllOrderQuery, Result<List<OrderDto>>>
    {
        private readonly IGenericRepository<Domain.AggregatesModel.OrderAggregate.Order, OrderDto> _genericRepository;

        public GetAllOrderQueryHandler(IGenericRepository<Domain.AggregatesModel.OrderAggregate.Order, OrderDto> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Result<List<OrderDto>>> Handle(GetAllOrderQuery request, CancellationToken cancellationToken)
        {
            var result = await _genericRepository.GetAllAsync();

            return result;
        }
    }
}
