using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Touride.Framework.Notification.Email.Fake;
using Touride.Framework.Notification.Email.FluentEmail.Implementation;
using Touride.Framework.Notification.Email.FluentEmail.Interface;
using Touride.Framework.Notification.Email.SendGrid;
using Touride.Framework.Notification.Email.SmtpClient;

namespace Touride.Framework.Notification.Email
{
    public static class EmailNotificationServiceCollectionExtensions
    {
        public static IServiceCollection AddSmtpClientEmailNotification(this IServiceCollection services, SmtpClientOptions options)
        {
            services.AddSingleton<IEmailNotification>(new SmtpClientEmailNotification(options));
            return services;
        }

        public static IServiceCollection AddFluentEmailNotification(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<IMailService, MailService>();

            services.AddFluentEmail(Configuration["EmailSettings:From"])
                .AddSmtpSender(new System.Net.Mail.SmtpClient()
                {
                    Host = Configuration["EmailSettings:Host"],
                    Port = Convert.ToInt32(Configuration["EmailSettings:Port"]),
                    Credentials = new NetworkCredential(Configuration["EmailSettings:UserName"],
                    Configuration["EmailSettings:Password"]),
                    EnableSsl = Convert.ToBoolean(Configuration["EmailSettings:EnableSsl"])
                });

            return services;
        }
        public static IServiceCollection AddFakeEmailNotification(this IServiceCollection services)
        {
            services.AddSingleton<IEmailNotification>(new FakeEmailNotification());
            return services;
        }

        public static IServiceCollection AddSendGridEmailNotification(this IServiceCollection services, SendGridOptions options)
        {
            services.AddSingleton<IEmailNotification>(new SendGridEmailNotification(options));
            return services;
        }

        public static IServiceCollection AddEmailNotification(this IServiceCollection services, EmailOptions options)
        {
            if (options.UsedFake())
            {
                services.AddFakeEmailNotification();
            }
            else if (options.UsedSmtpClient())
            {
                services.AddSmtpClientEmailNotification(options.SmtpClient);
            }
            else if (options.UsedSendGrid())
            {
                services.AddSendGridEmailNotification(options.SendGrid);
            }

            return services;
        }
    }
}
