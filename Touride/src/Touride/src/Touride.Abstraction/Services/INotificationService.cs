using Touride.Abstraction.Dtos;

namespace Touride.Abstraction.Services
{
    public interface INotificationService
    {
        Task Send(EmailMessageDto emailDto);
    }
}
