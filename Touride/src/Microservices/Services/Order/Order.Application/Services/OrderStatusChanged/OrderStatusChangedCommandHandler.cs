using AutoMapper;
using Dapr.Actors;
using Dapr.Actors.Client;
using Microsoft.Extensions.Logging;
using Order.Application.Actors;
using Order.Application.Models;
using Order.Domain.AggregatesModel.AddressAggregate;
using Order.Domain.AggregatesModel.OrderAggregate;
using Order.Domain.AggregatesModel.OrderItemAggregate;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.Abstractions.Data.Enums;
using Touride.Framework.Dapr.Abstractions;
using Touride.Framework.Data.Abstractions;
using Touride.Framework.MediatR.Configuration.Commands;

namespace Order.Application.Services.OrderStatusChanged
{
    public class OrderStatusChangedCommandHandler : ICommandHandler<OrderStatusChangedCommand, Result<OrderDto>>
    {
        private readonly IActorProxyFactory _actorProxyFactory;
        private readonly ILogger<OrderStatusChangedCommandHandler> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IGenericRepository<Domain.AggregatesModel.OrderAggregate.Order, OrderDto> _genericRepository;
        private readonly IMapper _mapper;
        private readonly IEventBus _eventBus;
        public OrderStatusChangedCommandHandler(
            IActorProxyFactory actorProxyFactory,
            ILogger<OrderStatusChangedCommandHandler> logger,
            IOrderRepository orderRepository,
            IMapper mapper,
            IEventBus eventBus,
            IGenericRepository<Domain.AggregatesModel.OrderAggregate.Order, OrderDto> genericRepository)
        {
            _actorProxyFactory = actorProxyFactory;
            _logger = logger;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _eventBus = eventBus;
            _genericRepository = genericRepository;
        }

        public async Task<Result<OrderDto>> Handle(OrderStatusChangedCommand request, CancellationToken cancellationToken)
        {

            var actorId = new ActorId(request.OrderId.ToString());
            var orderingProcess = _actorProxyFactory.CreateActorProxy<IOrderingProcessActor>(
                actorId,
                nameof(OrderingProcessActor));

            var state = await orderingProcess.GetOrderDetails();

            var readModelOrder = new Domain.AggregatesModel.OrderAggregate.Order()
            {
                Id = request.OrderId,
                OrderDate = state.OrderDate,
                OrderStatus = state.OrderStatus.Name,
                BuyerId = state.BuyerId,
                BuyerEmail = state.BuyerEmail,
                Description = state.Description,
                Address = new Address()
                {
                    Id = Guid.NewGuid(),
                    Street = state.Address.Street,
                    City = state.Address.City,
                    State = state.Address.State,
                    Country = state.Address.Country
                },
                OrderItems = state.OrderItems
                    .Select(itemState => new OrderItem()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = request.OrderId,
                        PictureFileName = itemState.PictureFileName,
                        ProductId = itemState.ProductId,
                        ProductName = itemState.ProductName,
                        UnitPrice = itemState.UnitPrice,
                        Units = itemState.Units
                    })
                    .ToList()
            };

            readModelOrder = _orderRepository.Insert(readModelOrder, InsertStrategy.InsertAll);

            var save = _orderRepository.SaveChanges();

            if (save > 0) await orderingProcess.OrderStatusChangedToSubmittedAsync();

            return new SuccessResult<OrderDto>(_mapper.Map<OrderDto>(readModelOrder))
            {
                Messages = new List<string>
                    {
                        "Sipariş kabul edildi, sipariş işlemi kaydedildi"
                    }
            };

        }
    }
}
