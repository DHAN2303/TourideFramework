using Touride.Abstraction.Services;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Touride.Api.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaticController : ControllerBase
    {
        private readonly IStaticBusinessService _staticBusinessService;
        public StaticController(IStaticBusinessService staticBusinessService)
        {
            _staticBusinessService = staticBusinessService;
        }
        [HttpGet("static")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAll(string key, Guid? entityId, string languageIndex, int? entity2Id = null)
        {
            var result = await _staticBusinessService.GetDynamicStringResource(key, entityId, languageIndex, entity2Id);

            return Ok(result);
        }
    }
}
