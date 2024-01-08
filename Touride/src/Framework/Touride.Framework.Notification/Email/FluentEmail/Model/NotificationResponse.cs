using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touride.Framework.Notification.Email.FluentEmail.Model
{
    public class NotificationResponse
    {
        public Guid Id { get; set; }
        public string? Message { get; set; }

        public string? NotificationId { get; set; }
        public bool IsSuccess { get; set; }
    }
}
