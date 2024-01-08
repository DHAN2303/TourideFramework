using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectName.Abstraction.Dtos;
using ProjectName.Domain.AggregatesModel.UserAggregate;
using Touride.Framework.Abstractions.Application.Models;

namespace ProjectName.Application.Handler.Queries.UserQueries.GetAllDevExtremeQueries
{
    public class GetAllDevExtremeQueryHandler : IRequestHandler<GetAllDevExtremeQuery, Result<LoadResult>>
    {
        private readonly IUserRepository _sampleRepository;
        private readonly IMapper _mapper;
        public GetAllDevExtremeQueryHandler(IUserRepository sampleRepository, IMapper mapper)
        {
            _sampleRepository = sampleRepository;
            _mapper = mapper;
        }
        public async Task<Result<LoadResult>> Handle(GetAllDevExtremeQuery request, CancellationToken cancellationToken)
        {
            var res = _sampleRepository.GetAll(include: p => p.Include(i => i.Addresses));

            var loadResult = DataSourceLoader.Load(res, request.loadOptions);

            IEnumerable<UserDto> map = loadResult.data.Cast<User>().Select(p => _mapper.Map<UserDto>(p));

            loadResult.data = map;

            return new SuccessResult<LoadResult>(loadResult);
        }
    }
}
