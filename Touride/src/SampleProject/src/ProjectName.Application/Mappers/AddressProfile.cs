using AutoMapper;
using ProjectName.Abstraction.Dtos;
using ProjectName.Domain.AggregatesModel.AddressAggregate;

namespace ProjectName.Application.Mappers
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
