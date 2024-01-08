using Dapr.Client;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.Abstractions.Caching;
using Touride.Framework.Abstractions.Caching.CacheManagement;
using Web.HttpAggregator.Abstraction.Dtos;
using Web.HttpAggregator.Abstraction.Services;

namespace Web.HttpAggregator.Application.Services
{
    public class BasketService : IBasketService
    {
        private readonly ICacheManager<BasketDto> _cacheManager;

        public BasketService(ICacheManager<BasketDto> cacheManager)
        {
            _cacheManager = cacheManager;
        }


        public async Task<Result<BasketCheckoutDto>> CheckoutAsync(BasketCheckoutDto basket)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/basket/checkout")
            {
                Content = JsonContent.Create(basket)
            };

            var httpClient = DaprClient.CreateInvokeHttpClient("basket-api");

            var response = await httpClient.SendAsync(request);


            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<SuccessResult<BasketCheckoutDto>>(content);
            }

            return new NoContentResult<BasketCheckoutDto>();
        }

        [Cache(key: "basketcreate", policyName: "Default", region: "Basket", keySuffixArgumentIndex: 0)]
        public virtual async Task<Result<BasketDto>> CreateAsync(BasketDto basket)
        {

            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/basket")
            {
                Content = JsonContent.Create(basket)
            };

            var httpClient = DaprClient.CreateInvokeHttpClient("basket-api");

            var response = await httpClient.SendAsync(request);


            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<SuccessResult<BasketDto>>(content);
            }

            return new NoContentResult<BasketDto>();
        }

        public async Task<Result<BasketDto>> GetBasketAsync(string buyerId)
        {
            var cacheKey = $"betbasket{buyerId}";

            var cache = _cacheManager.Get(cacheKey);

            if (cache == null)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"api/v1/basket?buyerId={buyerId}");

                var httpClient = DaprClient.CreateInvokeHttpClient("basket-api");

                var response = await httpClient.SendAsync(request);


                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<SuccessResult<BasketDto>>(content);

                    _cacheManager.AddOrUpdate(cacheKey, model.Data);

                    return JsonConvert.DeserializeObject<SuccessResult<BasketDto>>(content);
                }
            }

            return new SuccessResult<BasketDto>(cache);
            //return new NoContentResult<BasketDto>();
        }

        public async Task<Result<bool>> LoadTest()
        {
            for (int i = 0; i < 20; i++)
            {
                var buyerId = Guid.NewGuid().ToString();

                var basket = new BasketDto
                {
                    BuyerId = buyerId,
                    Items = new List<BasketItemDto>
                    {
                        new BasketItemDto{
                            ProductId =Guid.Parse("285d8680-a34f-4589-b6ef-526644cf75c0"),
                            ProductName = "Gömlek",
                            Quantity= 1,
                            UnitPrice = 10
                        },
                        new BasketItemDto{
                            ProductId =Guid.Parse("8b5680e4-fd9c-4ac2-8706-d42c38ed34b7"),
                            ProductName = "Pantalon",
                            Quantity= 1,
                            UnitPrice = 10
                        }
                    }


                };
                var basketCreate = await CreateAsync(basket);

                if (basketCreate.Success)
                {
                    var checkout = new BasketCheckoutDto
                    {
                        RequestId = Guid.NewGuid().ToString(),
                        CardHolderName = "Kafadar",
                        CardNumber = "1122 1122 3333 4444 5555",
                        CardSecurityCode = "325",
                        City = "Ankara",
                        Country = "Türkiye",
                        State = "Etimesgut",
                        CardExpiration = DateTime.Now,
                        Street = "5350 Cadde",
                        UserEmail = "cahityusuf@gmail.com",
                        UserId = basketCreate.Data.BuyerId
                    };

                    await CheckoutAsync(checkout);
                }
            }

            return new NoContentResult<bool>();
        }
    }
}
