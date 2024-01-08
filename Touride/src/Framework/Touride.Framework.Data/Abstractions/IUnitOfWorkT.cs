namespace Touride.Framework.Data.Abstractions
{
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : IUnitOfWork
    {
    }
}
