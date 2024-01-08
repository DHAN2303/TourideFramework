using Touride.Framework.Notification.Email.FluentEmail.Model;

namespace Touride.Framework.Notification.Email.FluentEmail.Interface
{
    public interface IMailService
    {
        Task<NotificationResponse> Send(EmailModel model);
    }
}
