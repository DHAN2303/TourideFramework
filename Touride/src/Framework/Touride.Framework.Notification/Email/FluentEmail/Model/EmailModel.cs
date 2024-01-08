namespace Touride.Framework.Notification.Email.FluentEmail.Model
{
    public class EmailModel
    {
        public EmailModel()
        {
            Attachments = new List<AttachmentModel>();
        }
        public Guid MailId { get; set; }
        public Guid CorrelationId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Bcc { get; set; }
        public string Cc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public bool UseTemplate { get; set; }
        public string HtmlTemplateFile { get; set; }
        public object HtmlValues { get; set; }
        public List<AttachmentModel> Attachments { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string? Password { get; set; }
    }

    public class AttachmentModel
    {
        public string Base64 { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
