using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Touride.Framework.Abstractions.Client;

namespace Touride.Framework.Data.AuditProperties
{
    public class AuditPropertyInterceptorManager : IAuditPropertyInterceptorManager
    {
        private readonly HashSet<IAuditPropertyInterceptor> Interceptors;
        public AuditPropertyInterceptorManager(IEnumerable<IAuditPropertyInterceptor> auditPropertyInterceptors)
        {
            Interceptors = new HashSet<IAuditPropertyInterceptor>();
            foreach (var interceptor in auditPropertyInterceptors)
            {
                if (interceptor.Enabled)
                {
                    Interceptors.Add(interceptor);
                }
            }
        }

        public void OnModelCreating(EntityTypeBuilder entityTypeBuilder)
        {
            foreach (var interceptor in Interceptors)
            {
                if (interceptor.ShoulIntercept(entityTypeBuilder.Metadata.ClrType))
                {
                    interceptor.OnModelCreating(entityTypeBuilder);
                }
            }
        }

        public void OnSave(IUserContextProvider clientInfoProvider, DateTime operationTime, EntityEntry entityEntry)
        {
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    {
                        OnInsert(clientInfoProvider, operationTime, entityEntry);
                        break;
                    }
                case EntityState.Modified:
                    {
                        OnUpdate(clientInfoProvider, operationTime, entityEntry);
                        break;
                    }
                case EntityState.Deleted:
                    {
                        OnDelete(clientInfoProvider, operationTime, entityEntry);
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
        }

        private void OnInsert(IUserContextProvider clientInfoProvider, DateTime operationTime, EntityEntry entityEntry)
        {
            foreach (var interceptor in Interceptors)
            {
                if (interceptor.ShoulIntercept(entityEntry.Metadata.ClrType))
                {
                    interceptor.OnInsert(clientInfoProvider, operationTime, entityEntry);
                }
            }
        }

        private void OnUpdate(IUserContextProvider clientInfoProvider, DateTime operationTime, EntityEntry entityEntry)
        {
            foreach (var interceptor in Interceptors)
            {
                if (interceptor.ShoulIntercept(entityEntry.Metadata.ClrType))
                {
                    interceptor.OnUpdate(clientInfoProvider, operationTime, entityEntry);
                }
            }
        }

        private void OnDelete(IUserContextProvider clientInfoProvider, DateTime operationTime, EntityEntry entityEntry)
        {
            foreach (var interceptor in Interceptors)
            {
                if (interceptor.ShoulIntercept(entityEntry.Metadata.ClrType))
                {
                    interceptor.OnDelete(clientInfoProvider, operationTime, entityEntry);
                }
            }
        }
    }
}
