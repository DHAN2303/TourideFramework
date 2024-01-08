using Basket.Abstraction.Dtos;
using Touride.Framework.Dapr.Events;

namespace Basket.Application.IntegrationEvents.Events
{
    public record UserCheckoutAcceptedIntegrationEvent(
        string UserId,
        string UserEmail,
        string City,
        string Street,
        string State,
        string Country,
        string CardNumber,
        string CardHolderName,
        DateTime CardExpiration,
        string CardSecurityNumber,
        Guid RequestId,
        BasketDto Basket)
        : IntegrationEvent;
}