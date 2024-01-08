using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Touride.Abstraction.Dtos;
using Touride.Abstraction.Services;
using Touride.Framework.Notification.Email.FluentEmail.Interface;
using Touride.Framework.Notification.Email.FluentEmail.Model;

namespace Touride.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMailService _mailService;
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration;
        public NotificationService(
            IMailService mailService,
            IConfiguration configuration,
            IHostingEnvironment env)
        {
            _mailService = mailService;
            _configuration = configuration;
            _env = env;
        }


        public async Task Send(EmailMessageDto model)
        {
            EmailModel emailModel = new EmailModel();
            emailModel.To = model.Tos;
            emailModel.Subject = model.Subject;
            emailModel.Body = model.Body;
            emailModel.Bcc = model.BCCs;
            emailModel.Cc = model.CCs;

            byte[] fileBytes = File.ReadAllBytes($"{_env.WebRootPath}/FinancialSample.xlsx");
            string base64String = Convert.ToBase64String(fileBytes);
            AttachmentModel attachment = new AttachmentModel { Base64 = base64String, ContentType = "application/vnd.ms-excel", FileName = "Financial Sample" };
            emailModel.Attachments.Add(attachment);

            await _mailService.Send(emailModel);

        }
    }
}
