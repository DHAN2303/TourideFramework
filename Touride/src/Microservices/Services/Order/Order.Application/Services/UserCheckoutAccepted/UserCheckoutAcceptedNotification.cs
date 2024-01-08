using MediatR;
using Order.Application.IntegrationEvents;

namespace Order.Application.Services.UserCheckoutAccepted
{
    public class UserCheckoutAcceptedNotification : INotification
    {
        public UserCheckoutAcceptedNotification(
                  string userId,
                  string userEmail,
                  string city,
                  string street,
                  string state,
                  string country,
                  string cartNumber,
                  string cartHolderName,
                  DateTime cardExpiration,
                  string cardSecurityNumber,
                  Guid requestId,
                  CustomerBasket customerBasket)
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
            CardSecurityNumber = cardSecurityNumber;
            RequestId = requestId;
            CustomerBasket = customerBasket;
        }
        public Guid RequestId { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime CardExpiration { get; set; }
        public string CardSecurityNumber { get; set; }
        public CustomerBasket CustomerBasket { get; set; }
    }
}
