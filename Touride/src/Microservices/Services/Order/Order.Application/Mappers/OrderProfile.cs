using AutoMapper;
using Order.Application.Models;

namespace Order.Application.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Domain.AggregatesModel.OrderAggregate.Order, OrderDto>().ReverseMap();
            CreateMap<Domain.AggregatesModel.AddressAggregate.Address, AddressDto>().ReverseMap();
            CreateMap<Domain.AggregatesModel.OrderItemAggregate.OrderItem, OrderItemDto>().ReverseMap();
        }
    }
}
