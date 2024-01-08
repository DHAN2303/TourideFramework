using MediatR;
using ProjectName.Abstraction.Dtos;
using ProjectName.Domain.AggregatesModel.UserAggregate;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.Data.Abstractions;

namespace ProjectName.Application.Handler.Queries.UserQueries.GetFirstAllDefaultQueries
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
