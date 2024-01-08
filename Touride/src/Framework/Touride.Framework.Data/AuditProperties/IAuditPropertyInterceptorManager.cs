using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Touride.Framework.Abstractions.Client;

namespace Touride.Framework.Data.AuditProperties
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuditPropertyInterceptorManager
    {
        void OnModelCreating(EntityTypeBuilder entityTypeBuilder);
        void OnSave(IUserContextProvider clientInfoProvider, DateTime operationTime, EntityEntry entityEntry);
    }
}
