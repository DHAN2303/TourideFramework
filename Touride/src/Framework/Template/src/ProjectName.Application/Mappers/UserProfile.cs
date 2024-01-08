using AutoMapper;
using ProjectName.Abstraction.Dtos;
using ProjectName.Abstraction.Models.TestModels;
using ProjectName.Application.Handler.Commands.CreateUser;
using ProjectName.Domain.AggregatesModel.UserAggregate;

namespace ProjectName.Application.Mappers
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
