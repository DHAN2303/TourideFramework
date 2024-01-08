using Dapr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ProjectName.Abstraction.IntegrationEvents;
using ProjectName.Application.Handler.Notifications;
using System.Threading.Tasks;

namespace ProjectName.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class SampleEventController : ControllerBase
    {
        private const string DAPR_PUBSUB_NAME = "pubsub";
        private readonly IMediator _mediator;

        public SampleEventController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("TestIntegrationEvent")]
        [Topic(DAPR_PUBSUB_NAME, "TestIntegrationEvent")]
        public async Task HandleAsync(TestIntegrationEvent integrationEvent)
        {
            await _mediator.Publish(new UserNotification(integrationEvent.Id, integrationEvent.Name));
        }
    }
}
