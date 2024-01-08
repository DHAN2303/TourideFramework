using AutoMapper;
using Touride.Abstraction.Dtos;
using Touride.Domain.AggregatesModel.AddressAggregate;

namespace Touride.Application.Mappers
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
