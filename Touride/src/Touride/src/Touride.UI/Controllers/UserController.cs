using Microsoft.AspNetCore.Mvc;
using Touride.UI.Mappers;
using Touride.UI.Services;

namespace Touride.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly TourideOpenApi _tourideOpenApi;
        private readonly IDevExtremeResultService<UserDto> _devExtremeResultService;
        public UserController(TourideOpenApi tourideOpenApi, IDevExtremeResultService<UserDto> devExtremeResultServicei)
        {
            _tourideOpenApi = tourideOpenApi;
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
            var res = await _tourideOpenApi.InsertAsync(user);

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromBody] DataSourceLoadOptions loadOptions)
        {
            try
            {
                DataSourceLoadOptions mapper = DataSourceLoadOptionsMapper.DataSourcemap(loadOptions);
                
                var result = await _tourideOpenApi.GetalldevextremeAsync(mapper);

                if (mapper.Group is not null && mapper.Group.Count() > 0)
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
