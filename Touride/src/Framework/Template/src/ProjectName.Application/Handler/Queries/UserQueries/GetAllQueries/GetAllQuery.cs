using MediatR;
using ProjectName.Abstraction.Dtos;
using Touride.Framework.Abstractions.Application.Models;

namespace ProjectName.Application.Handler.Queries.UserQueries.GetAllQueries
{
    public class GetAllQuery : IRequest<Result<List<UserDto>>>
    {
    }
}
