using Dapr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.IntegrationEvents.Events;
using Product.Application.Services.OrderStatusChangedToAwaitingStockValidation;
using Product.Application.Services.OrderStatusChangedToPaid;

namespace Product.API.Controllers
{
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class IntegrationEventController : ControllerBase
    {
        private const string DAPR_PUBSUB_NAME = "touride-pubsub";
        private readonly IMediator _mediator;

        public IntegrationEventController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("OrderStatusChangedToAwaitingStockValidation")]
        [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToAwaitingStockValidationIntegrationEvent))]
        public async Task HandleAsync(
        OrderStatusChangedToAwaitingStockValidationIntegrationEvent @event)
        {
            await _mediator.Publish(new OrderStatusChangedToAwaitingStockValidationNotification(
                    @event.OrderId,
                    @event.OrderStockItems
                ));
        }

        [HttpPost("OrderStatusChangedToPaid")]
        [Topic(DAPR_PUBSUB_NAME, "OrderStatusChangedToPaidIntegrationEvent")]
        public async Task HandleAsync(
        OrderStatusChangedToPaidIntegrationEvent @event)
        {
            var res = await _mediator.Send(new OrderStatusChangedToPaidCommand(
                        @event.OrderId,
                        @event.OrderStockItems
                    ));
            if (res.Success)
            {
                Console.WriteLine(res.Data[0].Name + "==>" + res.Data[0].AvailableStock);
                Console.WriteLine(res.Data[1].Name + "==>" + res.Data[1].AvailableStock);
            }
            else
            {
                Console.WriteLine("hata");
            }

        }
    }
}