using Basket.Application.IntegrationEvents.EventHandling;
using Basket.Application.IntegrationEvents.Events;
using Dapr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/v1/[controller]")]
[AllowAnonymous]
[ApiController]
public class IntegrationEventController : ControllerBase
{
    private const string DAPR_PUBSUB_NAME = "touride-pubsub";

    [HttpPost("OrderStatusChangedToSubmitted")]
    [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToSubmittedIntegrationEvent))]
    public Task HandleAsync(
        OrderStatusChangedToSubmittedIntegrationEvent @event,
        [FromServices] OrderStatusChangedToSubmittedIntegrationEventHandler handler)
        => handler.Handle(@event);
}
