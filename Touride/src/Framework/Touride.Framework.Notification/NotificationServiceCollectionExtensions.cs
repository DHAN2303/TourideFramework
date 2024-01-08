using Microsoft.Extensions.DependencyInjection;
using Touride.Framework.Notification.Email;
using Touride.Framework.Notification.Sms;

namespace Touride.Framework.Notification
{
    public static class NotificationServiceCollectionExtensions
    {
        public static IServiceCollection AddNotificationServices(this IServiceCollection services, NotificationOptions options)
        {
            services.AddEmailNotification(options.Email);

            services.AddSmsNotification(options.Sms);

            return services;
        }
    }
}
