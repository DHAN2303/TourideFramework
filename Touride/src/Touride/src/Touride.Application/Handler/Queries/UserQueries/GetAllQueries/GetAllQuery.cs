using MediatR;
using Touride.Abstraction.Dtos;
using Touride.Framework.Abstractions.Application.Models;

namespace Touride.Application.Handler.Queries.UserQueries.GetAllQueries
{
    public class GetAllQuery : IRequest<Result<List<UserDto>>>
    {
    }
}
