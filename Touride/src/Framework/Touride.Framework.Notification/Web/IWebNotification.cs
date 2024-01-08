using System.Threading;
using System.Threading.Tasks;

namespace Touride.Framework.Notification.Web
{
    public interface IWebNotification<T>
    {
        Task SendAsync(T message, CancellationToken cancellationToken = default);
    }
}
