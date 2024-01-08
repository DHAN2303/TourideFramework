
using MediatR;
using Microsoft.Extensions.Logging;

namespace ProjectName.Application.Handler.Notifications
{
    public class UserNotificationHandler : INotificationHandler<UserNotification>
    {
        private readonly ILogger<UserNotificationHandler> _logger;

        public UserNotificationHandler(ILogger<UserNotificationHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(UserNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("notification started");
        }
    }
}
