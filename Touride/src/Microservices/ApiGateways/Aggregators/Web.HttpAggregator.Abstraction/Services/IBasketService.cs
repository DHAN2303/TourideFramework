using Touride.Framework.Abstractions.Application.Models;
using Web.HttpAggregator.Abstraction.Dtos;

namespace Web.HttpAggregator.Abstraction.Services
{
    public interface IBasketService
    {
        Task<Result<BasketDto>> CreateAsync(BasketDto basket);
        Task<Result<BasketCheckoutDto>> CheckoutAsync(BasketCheckoutDto basket);
        Task<Result<BasketDto>> GetBasketAsync(string buyerId);
        Task<Result<bool>> LoadTest();
    }
}
