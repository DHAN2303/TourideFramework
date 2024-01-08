using ProjectName.Abstraction.Dtos;

namespace ProjectName.Abstraction.Services
{
    public interface INotificationService
    {
        Task Send(EmailMessageDto emailDto);
    }
}
