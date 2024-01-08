namespace ProjectName.UI.Helpers
{
    public class NotificationHelpers
    {
        public const string NotificationKey = "ProjectName.UI.Notification";

        public class Alert
        {
            public AlertType Type { get; set; }
            public string Message { get; set; }
            public string Title { get; set; }
        }

        public enum AlertType
        {
            Info,
            Success,
            Warning,
            Error
        }
    }
}