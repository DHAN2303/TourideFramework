using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Touride.Framework.Abstractions.Client;
using Touride.Framework.Abstractions.Data.AuditProperties;
using Touride.Framework.Data.Configuration;

namespace Touride.Framework.Data.AuditProperties
{
    public class HasCreatedByUserCodeInterceptor : IAuditPropertyInterceptor
    {
        private readonly UnitOfWorkOptions _options;
        public HasCreatedByUserCodeInterceptor(IOptions<UnitOfWorkOptions> options)
        {
            _options = options.Value;
        }
        public string PropertyName => "CreatedByUserCode";
        public bool Enabled { get => _options.EnableCreatedByUserCodeAuditField; }
        public bool ShoulIntercept(Type type)
        {
            return type.GetInterfaces().Contains(typeof(IHasCreatedByUserCode));
        }

        public void OnModelCreating(EntityTypeBuilder entityTypeBuilder)
        {
            var property = entityTypeBuilder.Metadata.GetProperties().FirstOrDefault(p => p.Name == PropertyName);
            if (property == null)
            {
                entityTypeBuilder.Property<string>(PropertyName).HasMaxLength(64);
            }
        }

        public void OnInsert(IUserContextProvider userContextInfoProvider, DateTime operationTime, EntityEntry entityEntry)
        {
            entityEntry.Property(PropertyName).CurrentValue = userContextInfoProvider.ErpUserId;
        }

        public void OnUpdate(IUserContextProvider userContextInfoProvider, DateTime operationTime, EntityEntry entityEntry)
        {
            return;
        }

        public void OnDelete(IUserContextProvider userContextInfoProvider, DateTime operationTime, EntityEntry entityEntry)
        {
            return;
        }
    }
}
