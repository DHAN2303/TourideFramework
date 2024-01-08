using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Touride.Framework.Abstractions.Client;
using Touride.Framework.Abstractions.Data.AuditProperties;
using Touride.Framework.Data.Configuration;

namespace Touride.Framework.Data.AuditProperties
{
    public class HasUpdatedByUserCodeInterceptor : IAuditPropertyInterceptor
    {
        private readonly UnitOfWorkOptions _options;
        public HasUpdatedByUserCodeInterceptor(IOptions<UnitOfWorkOptions> options)
        {
            _options = options.Value;
        }
        public string PropertyName => "UpdatedByUserCode";
        public bool Enabled { get => _options.EnableUpdatedByUserCodeAuditField; }
        public bool ShoulIntercept(Type type)
        {
            return type.GetInterfaces().Contains(typeof(IHasUpdatedByUserCode));
        }
        public void OnModelCreating(EntityTypeBuilder entityTypeBuilder)
        {
            var property = entityTypeBuilder.Metadata.GetProperties().FirstOrDefault(p => p.Name == PropertyName);
            if (property == null)
            {
                entityTypeBuilder.Property<string>(PropertyName).HasMaxLength(64);
            }
        }
        public void OnInsert(IUserContextProvider userContextProvider, DateTime operationTime, EntityEntry entityEntry)
        {
            return;
        }
        public void OnUpdate(IUserContextProvider userContextProvider, DateTime operationTime, EntityEntry entityEntry)
        {
            entityEntry.Property(PropertyName).CurrentValue = userContextProvider.ErpUserId;
        }

        public void OnDelete(IUserContextProvider userContextProvider, DateTime operationTime, EntityEntry entityEntry)
        {
            return;
        }
    }
}
