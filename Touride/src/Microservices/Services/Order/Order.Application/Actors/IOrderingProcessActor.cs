using Dapr.Actors;
using Order.Application.IntegrationEvents;

namespace Order.Application.Actors
{
    public interface IOrderingProcessActor : IActor
    {
        Task SubmitAsync(
            string buyerId,
            string buyerEmail,
            string street,
            string city,
            string state,
            string country,
            CustomerBasket basket);

        Task StockConfirmedSimulatedWorkDoneAsync();
        Task StockRejectedSimulatedWorkDoneAsync(List<Guid> rejectedProductIds);
        Task OrderStatusChangedToSubmittedAsync();
        Task PaymentSucceededSimulatedWorkDoneAsync();
        Task PaymentFailedSimulatedWorkDoneAsync();

        Task<bool> CancelAsync();

        Task<bool> ShipAsync();

        Task<OrderState> GetOrderDetails();
    }
}