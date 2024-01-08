using Microsoft.AspNetCore.Mvc;
using ProjectName.UI;
using ProjectName.UI.Mappers;
using ProjectName.UI.Services;

namespace ProjectName.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly ProjectNameOpenAI _projectNameOpenAI;
        private readonly IDevExtremeResultService<UserDto> _devExtremeResultService;
        public UserController(ProjectNameOpenAI projectNameOpenAI, IDevExtremeResultService<UserDto> devExtremeResultServicei)
        {
            _projectNameOpenAI = projectNameOpenAI;
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
            var res = await _projectNameOpenAI.InsertAsync(user);

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromBody] Touride.Framework.DevExtreme.DataSourceLoadOptions loadOptions)
        {
            try
            {
                var mapper = DataSourceLoadOptionsMapper.DataSourcemap(loadOptions);

                var result = await _projectNameOpenAI.GetalldevextremeAsync(mapper);

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
