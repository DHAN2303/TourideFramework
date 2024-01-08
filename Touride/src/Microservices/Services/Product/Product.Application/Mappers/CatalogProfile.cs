using AutoMapper;
using Product.Abstraction.Dtos;
using Product.Domain.AggregatesModel.CatalogItemAggregate;

namespace Product.Application.Mappers
{
    public class CatalogProfile : Profile
    {
        public CatalogProfile()
        {
            CreateMap<CatalogItem, CatalogItemDto>().ReverseMap();
        }
    }
}
