using MediatR;
using Touride.Abstraction.Dtos;
using Touride.Framework.Abstractions.Application.Models;

namespace Touride.Application.Handler.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<Result<UserDto>>
    {
        public CreateUserCommand(
            Guid id,
            string name,
            string surname,
            string phone,
            string gender,
            bool isDeleted,
            List<AddressDto> addresses)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Phone = phone;
            Gender = gender;
            IsDeleted = isDeleted;
            Addresses = addresses;
        }
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public bool IsDeleted { get; set; } = false;

        public List<AddressDto> Addresses { get; set; }
    }
}
