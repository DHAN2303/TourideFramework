using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Touride.Framework.Abstractions.Client;

namespace Touride.Framework.Data.AuditProperties
{
    public interface IAuditPropertyInterceptor
    {
        bool Enabled { get; }
        string PropertyName { get; }
        bool ShoulIntercept(Type type);
        void OnModelCreating(EntityTypeBuilder entityTypeBuilder);
        void OnInsert(IUserContextProvider clientInfoProvider, DateTime operationTime, EntityEntry entityEntry);
        void OnDelete(IUserContextProvider clientInfoProvider, DateTime operationTime, EntityEntry entityEntry);
        void OnUpdate(IUserContextProvider clientInfoProvider, DateTime operationTime, EntityEntry entityEntry);
    }
}
