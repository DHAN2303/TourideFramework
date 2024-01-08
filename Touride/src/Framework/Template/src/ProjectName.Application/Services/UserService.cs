using AutoMapper;
using DevExtreme.AspNet.Data.ResponseModel;
using ProjectName.Abstraction.Dtos;
using ProjectName.Abstraction.Models.TestModels;
using ProjectName.Abstraction.Services;
using ProjectName.Domain.AggregatesModel.UserAggregate;
using System.Diagnostics;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.Abstractions.Caching;
using Touride.Framework.Abstractions.Caching.CacheManagement;
using Touride.Framework.Abstractions.Data.TransactionManagement;

namespace ProjectName.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ICacheManager<string> _cacheManager;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            ICacheManager<string> cacheManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _cacheManager = cacheManager;
        }

        public async Task<Result<List<UserDto>>> GetAll()
        {
            var result = _userRepository.GetAll();

            return new SuccessResult<List<UserDto>>(_mapper.Map<List<UserDto>>(result));
        }

        public async Task<Result<List<LoadResult>>> GetAllDevExtreme()
        {
            throw new NotImplementedException();
        }

        [Transactional]
        public virtual async Task<Result<UserDto>> Insert(CreateTestModel createTestModel)
        {
            var user = _mapper.Map<User>(createTestModel);

            var savemodel = _userRepository.Insert(user);

            var save = await _userRepository.SaveChangesAsync();

            if (save > 0)
            {
                //await _eventBus.PublishAsync(new TestIntegrationEvent(request.Id, request.Name));

                return new SuccessResult<UserDto>(_mapper.Map<UserDto>(savemodel));
            }
            return new NoContentResult<UserDto>();
        }

        public virtual async Task<string> PerformanceTest2(string key)
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var cacheRes = _cacheManager.Get(key: key, region: "Localization");
            //await  _daprStateStore.GetStateAsync<string>(STATESTORENAME, key);
            sw.Stop();

            return cacheRes + " / " + sw.ElapsedMilliseconds.ToString();

        }
    }
}
