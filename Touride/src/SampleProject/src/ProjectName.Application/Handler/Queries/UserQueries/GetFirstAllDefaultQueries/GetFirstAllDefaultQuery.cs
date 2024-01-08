using MediatR;
using ProjectName.Abstraction.Dtos;
using Touride.Framework.Abstractions.Application.Models;

namespace ProjectName.Application.Handler.Queries.UserQueries.GetFirstAllDefaultQueries
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
