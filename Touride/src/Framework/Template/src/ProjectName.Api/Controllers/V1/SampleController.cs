using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ProjectName.Abstraction.Dtos;
using ProjectName.Abstraction.Models.TestModels;
using ProjectName.Abstraction.Services;
using ProjectName.Application.Handler.Commands.CreateUser;
using ProjectName.Application.Handler.Queries.UserQueries.GetAllDevExtremeQueries;
using ProjectName.Application.Handler.Queries.UserQueries.GetAllQueries;
using ProjectName.Application.Handler.Queries.UserQueries.GetFirstAllDefaultQueries;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Touride.Framework.Api.Application.Extensions;
using Touride.Framework.DevExtreme;

namespace ProjectName.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserService _userService;
        public SampleController(IMediator mediator, IUserService userService)
        {
            _mediator = mediator;
            _userService = userService;
        }

        [HttpPost("insert")]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Insert(CreateTestModel insert)
        {
            var result = await _mediator.Send(new CreateUserCommand(
                Guid.Empty,
                insert.Name,
                insert.Surname,
                insert.Phone,
                insert.Gender,
                insert.isDeleted,
                insert.Addresses));

            return Ok(result);
        }

        [HttpGet("getall")]
        [ProducesResponseType(typeof(List<UserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllQuery());

            return Ok(result);
        }

        [HttpGet("firstalldefault")]
        [ProducesResponseType(typeof(List<UserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetFirstAllDefault([FromQuery] Guid id)
        {
            var result = await _mediator.Send(new GetFirstAllDefaultQuery(id));

            return Ok(result);
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
