using MediatR;
using Touride.Abstraction.Dtos;
using Touride.Abstraction.Services;
using Touride.Framework.Abstractions.Application.Models;

namespace Touride.Application.Handler.Queries.UserQueries.GetAllQueries
{
    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, Result<List<UserDto>>>
    {
        private readonly IUserService _sampleService;

        public GetAllQueryHandler(IUserService sampleService)
        {
            _sampleService = sampleService;
        }

        public async Task<Result<List<UserDto>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var result = await _sampleService.GetAll();

            return result;
        }
    }
}
