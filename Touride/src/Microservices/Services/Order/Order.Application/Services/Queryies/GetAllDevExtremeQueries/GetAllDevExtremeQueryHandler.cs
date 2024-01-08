using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Application.Models;
using Order.Domain.AggregatesModel.OrderAggregate;
using Touride.Framework.Abstractions.Application.Models;

namespace Order.Application.Services.Queryies.GetAllDevExtremeQueries
{
    public class GetAllDevExtremeQueryHandler : IRequestHandler<GetAllDevExtremeQuery, Result<LoadResult>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public GetAllDevExtremeQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<Result<LoadResult>> Handle(GetAllDevExtremeQuery request, CancellationToken cancellationToken)
        {
            var res = _orderRepository.GetAll(include: p => p.Include(i => i.Address).Include(i => i.OrderItems));

            var loadResult = DataSourceLoader.Load(res, request.loadOptions);

            IEnumerable<OrderDto> map = loadResult.data.Cast<Domain.AggregatesModel.OrderAggregate.Order>().Select(p => _mapper.Map<OrderDto>(p));

            loadResult.data = map;

            return new SuccessResult<LoadResult>(loadResult);
        }
    }
}
