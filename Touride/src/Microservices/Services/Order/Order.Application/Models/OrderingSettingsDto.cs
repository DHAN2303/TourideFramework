namespace Order.Application.Models
{
    public class OrderingSettingsDto
    {
        public int GracePeriodTime { get; set; }

        public bool SendConfirmationEmail { get; set; }
    }
}
