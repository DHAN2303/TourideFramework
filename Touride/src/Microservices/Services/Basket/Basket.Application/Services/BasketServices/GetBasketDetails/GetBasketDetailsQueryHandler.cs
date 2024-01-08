using Basket.Abstraction.Dtos;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.Dapr.Abstractions;
using Touride.Framework.MediatR.Configuration.Queries;

namespace Basket.Application.Services.BasketServices.GetBasketDetails
{
    public class GetBasketDetailsQueryHandler : IQueryHandler<GetBasketDetailsQuery, Result<BasketDto>>
    {
        private readonly IDaprStateStore _daprStateStore;
        public GetBasketDetailsQueryHandler(IDaprStateStore daprStateStore)
        {
            _daprStateStore = daprStateStore;
        }
        private const string DAPR_PUBSUB_NAME = "touride-pubsub";
        public async Task<Result<BasketDto>> Handle(GetBasketDetailsQuery request, CancellationToken cancellationToken)
        {
            var aa = await _daprStateStore.GetStateAsync<BasketDto>(DAPR_PUBSUB_NAME, request.CustomerId);

            //await _daprStateStore.UpdateStateAsync<GetBasketDetailsQuery>(request.CustomerId, request);

            return new SuccessResult<BasketDto>(await _daprStateStore.GetStateAsync<BasketDto>(DAPR_PUBSUB_NAME, request.CustomerId))
            {
                Messages = new List<string>
                    {
                        "Ürün sepete kaydedildi"
                    }
            };
        }
    }
}