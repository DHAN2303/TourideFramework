using AutoMapper;
using Order.Application.Models;
using Order.Domain.AggregatesModel.OrderAggregate;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.MediatR.Configuration.Commands;

namespace Order.Application.Services.OrderUpdateStatus
{
    public class UpdateOrderStatusCommandHandler : ICommandHandler<UpdateOrderStatusCommand, Result<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<Result<OrderDto>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetFirstOrDefaultAsync(predicate: p => p.Id == request.Id);

            if (order is not null)
            {
                order.OrderStatus = request.OrderStatus;
                order.Description = request.Description;

                _orderRepository.Update(order);

                _orderRepository.SaveChanges();

                return new SuccessResult<OrderDto>(_mapper.Map<OrderDto>(order))
                {
                    Messages = new List<string>
                    {
                        "Sipariş kabul edildi, sipariş işlemi kaydedildi"
                    }
                };
            }

            return new NoContentResult<OrderDto>();
        }
    }
}
