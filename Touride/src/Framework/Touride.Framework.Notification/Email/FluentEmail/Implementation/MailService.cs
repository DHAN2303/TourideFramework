using FluentEmail.Core;
using Touride.Framework.Notification.Email.FluentEmail.Interface;
using Touride.Framework.Notification.Email.FluentEmail.Model;
using Attachment = FluentEmail.Core.Models.Attachment;

namespace Touride.Framework.Notification.Email.FluentEmail.Implementation
{
    public class MailService : IMailService
    {
        private readonly IFluentEmail _fluentEmail;

        public MailService(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }

        public async Task<NotificationResponse> Send(EmailModel model)
        {
            NotificationResponse response = new NotificationResponse();

            try
            {

                if (string.IsNullOrEmpty(model.To))
                    return new NotificationResponse()
                    {
                        Id = Guid.NewGuid(),
                        IsSuccess = false,
                        Message = "Invalid Model",
                    };

                if (model.Bcc != null)
                {
                    _fluentEmail.BCC(model.Bcc);
                }

                if (model.Cc != null)
                {
                    _fluentEmail.CC(model.Cc);
                }

                model.Subject = model.Subject.Replace("\r", "").Replace("\n", "");

                if (string.IsNullOrEmpty(model.Subject))
                {
                    model.Subject = "test";
                }

                _fluentEmail.Subject(model.Subject);

                if (model.Attachments != null)
                {
                    foreach (var attachment in model.Attachments)
                    {
                        var bytes = Convert.FromBase64String(attachment.Base64);
                        var contents = new StreamContent(new MemoryStream(bytes));

                        _fluentEmail.Attach(new Attachment()
                        {
                            ContentType = attachment.ContentType,
                            Data = contents.ReadAsStream(),
                            Filename = attachment.FileName
                        });
                    }
                }

                await _fluentEmail.To(model.To)
                                  .Body(model.Body, model.IsHtml).SendAsync();

                response.Id = model.MailId;
                response.IsSuccess = true;


                return response;
            }
            catch (Exception ex)
            {
                response.Id = model.MailId;
                response.IsSuccess = false;
                response.Message = "Message  :" + ex.Message + " " + "Source   :" + ex.Source + "InnerException   :" + ex.InnerException.Message;

                return response;
            }
        }
    }
}