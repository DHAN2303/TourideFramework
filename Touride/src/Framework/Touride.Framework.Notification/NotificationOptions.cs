using Touride.Framework.Notification.Email;
using Touride.Framework.Notification.Sms;
using Touride.Framework.Notification.Web;

namespace Touride.Framework.Notification
{
    public class NotificationOptions
    {
        public EmailOptions Email { get; set; }

        public SmsOptions Sms { get; set; }

        public WebOptions Web { get; set; }
    }
}
