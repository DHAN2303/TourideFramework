using System.Threading;
using System.Threading.Tasks;

namespace Touride.Framework.Notification.Sms.Fake
{
    public class FakeSmsNotification : ISmsNotification
    {
        public Task SendAsync(ISmsMessage smsMessage, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
