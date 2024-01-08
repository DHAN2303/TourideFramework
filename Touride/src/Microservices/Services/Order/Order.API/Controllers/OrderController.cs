using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Services.Queryies.GetAllDevExtremeQueries;
using Order.Application.Services.Queryies.GetAllOrder;
using System.Net;
using Touride.Framework.DevExtreme;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllOrderQuery()));
        }

        [HttpPost("getalldevextreme")]
        [ProducesResponseType(typeof(LoadResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllFilter([FromBody] DataSourceLoadOptions loadOptions)
        {
            var result = await _mediator.Send(new GetAllDevExtremeQuery { loadOptions = loadOptions });

            return Ok(result.Data);
        }
    }
}
