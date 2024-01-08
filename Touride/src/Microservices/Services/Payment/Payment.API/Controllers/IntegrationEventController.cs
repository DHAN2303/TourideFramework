using Dapr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.IntegrationEvents.EventHandling;
using Payment.Application.IntegrationEvents.Events;

namespace Payment.API.Controllers
{
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class IntegrationEventController : ControllerBase
    {
        private const string DAPR_PUBSUB_NAME = "touride-pubsub";

        [HttpPost("OrderStatusChangedToValidated")]
        [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToValidatedIntegrationEvent))]
        public Task HandleAsync(
            OrderStatusChangedToValidatedIntegrationEvent @event,
            [FromServices] OrderStatusChangedToValidatedIntegrationEventHandler handler) =>
            handler.Handle(@event);
    }
}