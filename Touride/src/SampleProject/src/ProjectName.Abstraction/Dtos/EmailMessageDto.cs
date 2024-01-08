using Touride.Framework.Notification.Email;
using Touride.Framework.Notification.Email.FluentEmail.Model;

namespace ProjectName.Abstraction.Dtos
{
    public class EmailMessageDto : IEmailMessage
    {
        public EmailMessageDto()
        {

        }
        public EmailMessageDto(string from, string tos, string cCs, string bCCs, string subject, string body, List<AttachmentModel> attachments)
        {
            Tos = tos;
            CCs = cCs;
            BCCs = bCCs;
            Subject = subject;
            Body = body;
            Attachments = attachments;
        }

        public string From { get; set; }
        public string Tos { get; set; }
        public string CCs { get; set; }
        public string BCCs { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<AttachmentModel> Attachments { get; set; }
    }
}
