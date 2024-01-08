using AutoMapper;
using Touride.Abstraction.Dtos;
using Touride.Abstraction.Models.TestModels;
using Touride.Application.Handler.Commands.CreateUser;
using Touride.Domain.AggregatesModel.UserAggregate;

namespace Touride.Application.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreateTestModel, CreateUserCommand>().ReverseMap();
            CreateMap<CreateTestModel, User>().ReverseMap();
        }
    }
}
