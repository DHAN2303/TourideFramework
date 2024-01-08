

using Basket.Abstraction.Dtos;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.MediatR.Configuration.Queries;

namespace Basket.Application.Services.BasketServices.GetBasketDetails
{
    public class GetBasketDetailsQuery : IQuery<Result<BasketDto>>
    {
        public GetBasketDetailsQuery(string customerId)
        {
            CustomerId = customerId;
        }

        public string CustomerId { get; }
    }
}