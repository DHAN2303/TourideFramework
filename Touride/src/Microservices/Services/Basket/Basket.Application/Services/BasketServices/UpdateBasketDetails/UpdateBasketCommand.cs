using Basket.Abstraction.Dtos;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.MediatR.Configuration.Commands;

namespace Basket.Application.Services.BasketServices.UpdateBasketDetails
{
    public class UpdateBasketCommand : ICommand<Result<BasketDto>>
    {

        public UpdateBasketCommand(string buyerId, List<BasketItemDto> items)
        {
            BuyerId = buyerId;
            Items = items;
        }

        public Guid Id => Guid.NewGuid();

        public string BuyerId { get; set; } = "";
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();

    }
}
