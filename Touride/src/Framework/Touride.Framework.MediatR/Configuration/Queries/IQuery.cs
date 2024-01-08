using MediatR;

namespace Touride.Framework.MediatR.Configuration.Queries
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {

    }
}