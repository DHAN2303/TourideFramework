using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Touride.Framework.Abstractions.Client;
using Touride.Framework.Abstractions.Data.AuditProperties;
using Touride.Framework.Data.Configuration;

namespace Touride.Framework.Data.AuditProperties
{
    /// <summary>
    /// UpdatedBy özelliği olup olmadığını kontrol eden interceptor.
    /// Eğer IHasUpdatedBy varsa araya girip alanın oluşmasını sağlar ve değerini atar.
    /// </summary>
    public class HasUpdatedByInterceptor : IAuditPropertyInterceptor
    {
        private readonly UnitOfWorkOptions _options;
        public HasUpdatedByInterceptor(IOptions<UnitOfWorkOptions> options)
        {
            _options = options.Value;
        }
        public string PropertyName => "UpdatedBy";
        public bool Enabled { get => _options.EnableUpdatedByAuditField; }
        public bool ShoulIntercept(Type type)
        {
            return type.GetInterfaces().Contains(typeof(IHasUpdatedBy));
        }
        public void OnModelCreating(EntityTypeBuilder entityTypeBuilder)
        {
            var property = entityTypeBuilder.Metadata.GetProperties().FirstOrDefault(p => p.Name == PropertyName);
            if (property == null)
            {
                entityTypeBuilder.Property<string>(PropertyName).HasMaxLength(64); ;
            }
        }
        public void OnInsert(IUserContextProvider clientInfoProvider, DateTime operationTime, EntityEntry entityEntry)
        {
            return;
        }
        public void OnUpdate(IUserContextProvider clientInfoProvider, DateTime operationTime, EntityEntry entityEntry)
        {
            entityEntry.Property(PropertyName).CurrentValue = clientInfoProvider.UserId;
        }

        public void OnDelete(IUserContextProvider clientInfoProvider, DateTime operationTime, EntityEntry entityEntry)
        {
            return;
        }
    }
}
