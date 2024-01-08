using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Touride.Framework.Api.Application.Extensions;
using Web.HttpAggregator.Abstraction.Dtos;
using Web.HttpAggregator.Abstraction.Services;

namespace Web.HttpAggregator.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class BasketController : ControllerBase
    {

        private readonly IBasketService _basket;
        public BasketController(IBasketService basket)
        {
            _basket = basket;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BasketDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateBasketAsync(
            [FromBody] BasketDto basket)
        {
            return Ok(await _basket.CreateAsync(basket));
        }

        [HttpPost("checkout")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BasketCheckoutDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> BasketCheckoutAsync(
        [FromBody] BasketCheckoutDto basket,
        [FromHeader(Name = "Correlation-Id")] string requestId)
        {

            var reqid = requestId.ToString();

            return Ok(await _basket.CheckoutAsync(basket));
        }

        [HttpGet("getbasket")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BasketCheckoutDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetBasketAsync(string buyerId)
        {
            return this.FromResult(await _basket.GetBasketAsync(buyerId));
            //return Ok(await _basket.GetBasketAsync(buyerId));
        }

        [HttpGet("test")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BasketCheckoutDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Test()
        {
            return Ok(await _basket.LoadTest());
        }
    }
}