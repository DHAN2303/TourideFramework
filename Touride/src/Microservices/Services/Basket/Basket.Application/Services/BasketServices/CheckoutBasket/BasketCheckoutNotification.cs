using MediatR;

namespace Basket.Application.Services.BasketServices.CheckoutBasket
{
    public class BasketCheckoutNotification : INotification
    {
        public BasketCheckoutNotification(
            string requestId,
            string userId,
            string userEmail,
            string city,
            string street,
            string state,
            string country,
            string cartNumber,
            string cartHolderName,
            DateTime cardExpiration,
            string cardSecurityCode)
        {
            UserId = userId;
            UserEmail = userEmail;
            City = city;
            Street = street;
            State = state;
            Country = country;
            CardNumber = cartNumber;
            CardHolderName = cartHolderName;
            CardExpiration = cardExpiration;
            CardSecurityCode = cardSecurityCode;
            RequestId = requestId;
        }
        public string RequestId { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime CardExpiration { get; set; }
        public string CardSecurityCode { get; set; }
    }
}