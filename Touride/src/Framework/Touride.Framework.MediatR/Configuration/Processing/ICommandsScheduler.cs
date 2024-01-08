using Touride.Framework.MediatR.Configuration.Commands;

namespace Touride.Framework.MediatR.Configuration.Processing
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync<T>(ICommand<T> command);
    }
}