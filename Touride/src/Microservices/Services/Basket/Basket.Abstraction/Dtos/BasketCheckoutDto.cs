namespace Basket.Abstraction.Dtos
{
    public record BasketCheckoutDto(
        string RequestId,
        string UserId,
        string UserEmail,
        string City,
        string Street,
        string State,
        string Country,
        string CardNumber,
        string CardHolderName,
        DateTime CardExpiration,
        string CardSecurityCode,
        BasketDto Basket
    );
}
