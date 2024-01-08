using Basket.Abstraction.Dtos;
using Basket.Application.Services.BasketServices.CheckoutBasket;
using Basket.Application.Services.BasketServices.GetBasketDetails;
using Basket.Application.Services.BasketServices.UpdateBasketDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Touride.Framework.Api.Application.Extensions;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BasketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BasketDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetBasketAsync([FromQuery] string buyerId)
        {
            var res = await _mediator.Send(new GetBasketDetailsQuery(buyerId));

            return this.FromResult(res);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketDto>> UpdateBasketAsync([FromBody] BasketDto value)
        {
            var res = await _mediator.Send(new UpdateBasketCommand(value.BuyerId, value.Items));

            return this.FromResult(res);
        }


        [HttpPost("checkout")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CheckoutAsync(
        [FromBody] BasketCheckoutDto basketCheckout)
        {

            await _mediator.Publish(new BasketCheckoutNotification(
                            basketCheckout.RequestId,
                            basketCheckout.UserId,
                            basketCheckout.UserEmail,
                            basketCheckout.City,
                            basketCheckout.Street,
                            basketCheckout.State,
                            basketCheckout.Country,
                            basketCheckout.CardNumber,
                            basketCheckout.CardHolderName,
                            basketCheckout.CardExpiration,
                            basketCheckout.CardSecurityCode
                        ));

            return Accepted();
        }
    }
}