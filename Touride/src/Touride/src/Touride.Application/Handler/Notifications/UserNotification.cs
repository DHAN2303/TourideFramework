using MediatR;

namespace Touride.Application.Handler.Notifications
{
    public class UserNotification : INotification
    {
        public UserNotification(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
