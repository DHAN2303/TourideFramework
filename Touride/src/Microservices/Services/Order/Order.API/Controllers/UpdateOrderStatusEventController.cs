using Dapr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Order.Application.IntegrationEvents;
using Order.Application.Models;
using Order.Application.Services.OrderStatusChanged;
using Order.Application.Services.OrderUpdateStatus;

namespace Order.API.Controllers
{
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class UpdateOrderStatusEventController : ControllerBase
    {
        private const string DAPR_PUBSUB_NAME = "touride-pubsub";
        private readonly IMediator _mediator;
        public UpdateOrderStatusEventController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("OrderStatusChangedToSubmitted")]
        [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToSubmittedIntegrationEvent))]
        public async Task HandleAsync(
            OrderStatusChangedToSubmittedIntegrationEvent integrationEvent,
            [FromServices] IOptions<OrderingSettingsDto> settings)
        {
            await _mediator.Send(new OrderStatusChangedCommand(
                integrationEvent.OrderId,
                integrationEvent.OrderStatus,
                integrationEvent.BuyerId,
                integrationEvent.BuyerEmail
            ));

        }

        [HttpPost("OrderStatusChangedToAwaitingStockValidation")]
        [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToAwaitingStockValidationIntegrationEvent))]
        public Task HandleAsync(
            OrderStatusChangedToAwaitingStockValidationIntegrationEvent integrationEvent)
        {
            return UpdateReadModelAndSendNotificationAsync(integrationEvent.OrderId,
                integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerId);
        }

        [HttpPost("OrderStatusChangedToValidated")]
        [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToValidatedIntegrationEvent))]
        public Task HandleAsync(
            OrderStatusChangedToValidatedIntegrationEvent integrationEvent)
        {
            return UpdateReadModelAndSendNotificationAsync(integrationEvent.OrderId,
                integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerId);
        }

        [HttpPost("OrderStatusChangedToPaid")]
        [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToPaidIntegrationEvent))]
        public Task HandleAsync(
            OrderStatusChangedToPaidIntegrationEvent integrationEvent)
        {
            return UpdateReadModelAndSendNotificationAsync(integrationEvent.OrderId,
                integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerId);
        }

        [HttpPost("OrderStatusChangedToShipped")]
        [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToShippedIntegrationEvent))]
        public Task HandleAsync(
            OrderStatusChangedToShippedIntegrationEvent integrationEvent)
        {
            return UpdateReadModelAndSendNotificationAsync(integrationEvent.OrderId,
                integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerId);
        }

        [HttpPost("OrderStatusChangedToCancelled")]
        [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToCancelledIntegrationEvent))]
        public Task HandleAsync(
            OrderStatusChangedToCancelledIntegrationEvent integrationEvent)
        {
            return UpdateReadModelAndSendNotificationAsync(integrationEvent.OrderId,
                integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerId);
        }

        private async Task UpdateReadModelAndSendNotificationAsync(
            Guid orderId, string orderStatus, string description, string buyerId)
        {
            await _mediator.Send(new UpdateOrderStatusCommand(orderId, description, orderStatus));
        }

    }
}