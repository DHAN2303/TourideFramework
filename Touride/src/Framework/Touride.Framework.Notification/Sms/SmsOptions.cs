using Touride.Framework.Notification.Sms.Azure;
using Touride.Framework.Notification.Sms.Twilio;

namespace Touride.Framework.Notification.Sms
{
    public class SmsOptions
    {
        public string Provider { get; set; }

        public TwilioOptions Twilio { get; set; }

        public AzureOptions Azure { get; set; }

        public bool UsedFake()
        {
            return Provider == "Fake";
        }

        public bool UsedTwilio()
        {
            return Provider == "Twilio";
        }

        public bool UsedAzure()
        {
            return Provider == "Azure";
        }
    }
}
