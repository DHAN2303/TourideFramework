﻿using Microsoft.Extensions.DependencyInjection;
using Touride.Framework.Notification.Sms.Azure;
using Touride.Framework.Notification.Sms.Fake;
using Touride.Framework.Notification.Sms.Twilio;

namespace Touride.Framework.Notification.Sms
{
    public static class SmsNotificationServiceCollectionExtensions
    {
        public static IServiceCollection AddTwilioSmsNotification(this IServiceCollection services, TwilioOptions options)
        {
            services.AddSingleton<ISmsNotification>(new TwilioSmsNotification(options));
            return services;
        }

        public static IServiceCollection AddAzureSmsNotification(this IServiceCollection services, AzureOptions options)
        {
            services.AddSingleton<ISmsNotification>(new AzureSmsNotification(options));
            return services;
        }

        public static IServiceCollection AddFakeSmsNotification(this IServiceCollection services)
        {
            services.AddSingleton<ISmsNotification>(new FakeSmsNotification());
            return services;
        }

        public static IServiceCollection AddSmsNotification(this IServiceCollection services, SmsOptions options)
        {
            if (options.UsedFake())
            {
                services.AddFakeSmsNotification();
            }
            else if (options.UsedTwilio())
            {
                services.AddTwilioSmsNotification(options.Twilio);
            }
            else if (options.UsedAzure())
            {
                services.AddAzureSmsNotification(options.Azure);
            }

            return services;
        }
    }
}
