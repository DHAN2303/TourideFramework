using MediatR;
using Touride.Abstraction.Dtos;
using Touride.Domain.AggregatesModel.UserAggregate;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.Data.Abstractions;

namespace Touride.Application.Handler.Queries.UserQueries.GetFirstAllDefaultQueries
{
    public class GetFirstAllDefaultQueryHandler : IRequestHandler<GetFirstAllDefaultQuery, Result<List<UserDto>>>
    {
        private readonly IGenericRepository<User, UserDto> _genericRepository;
        public GetFirstAllDefaultQueryHandler(IGenericRepository<User, UserDto> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Result<List<UserDto>>> Handle(GetFirstAllDefaultQuery request, CancellationToken cancellationToken)
        {
            return _genericRepository.GetAll(false, predicate: p => p.Id == request.Id);
        }
    }
}
