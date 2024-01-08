using MediatR;
using Touride.Abstraction.Dtos;
using Touride.Framework.Abstractions.Application.Models;

namespace Touride.Application.Handler.Queries.UserQueries.GetFirstAllDefaultQueries
{
    public class GetFirstAllDefaultQuery : IRequest<Result<List<UserDto>>>
    {
        public GetFirstAllDefaultQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
