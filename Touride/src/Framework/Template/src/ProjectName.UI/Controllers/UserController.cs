using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectName.Api;
using ProjectName.UI.Mappers;
using ProjectName.UI.Services;

namespace ProjectName.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly ProjectNameApi _projectNameApi;
        private readonly IDevExtremeResultService<UserDto> _devExtremeResultService;
        public UserController(ProjectNameApi projectNameApi, IDevExtremeResultService<UserDto> devExtremeResultServicei)
        {
            _projectNameApi = projectNameApi;
            _devExtremeResultService = devExtremeResultServicei;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Management()
        {

            return View();
        }

        [HttpGet("insert")]
        public async Task<IActionResult> Insert([FromBody] CreateTestModel user)
        {
            var res = await _projectNameApi.InsertAsync(user);

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromBody] Touride.Framework.DevExtreme.DataSourceLoadOptions loadOptions)
        {
            try
            {
                var mapper = DataSourceLoadOptionsMapper.DataSourcemap(loadOptions);

                var result = await _projectNameApi.GetalldevextremeAsync(mapper);

                if (mapper.Group is not null && mapper.Group.Count > 0)
                {
                    return Ok(_devExtremeResultService.GetGroupResult(mapper, result));
                }

                return Ok(_devExtremeResultService.GetLoadResult(mapper, result));
                //return mapper.Group is not null && mapper.Group.Count > 0
                //    ? Ok(_devExtremeResultService.GetGroupResult(mapper, result))
                //    : Ok(_devExtremeResultService.GetLoadResult(mapper, result));
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
