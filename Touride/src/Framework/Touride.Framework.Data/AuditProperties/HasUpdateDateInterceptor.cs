﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Touride.Framework.Abstractions.Client;
using Touride.Framework.Abstractions.Data.AuditProperties;
using Touride.Framework.Data.Configuration;

namespace Touride.Framework.Data.AuditProperties
{
    /// <summary>
    /// UpdateDate özelliği olup olmadığını kontrol eden interceptor.
    /// Eğer IHasUpdateDate varsa araya girip alanın oluşmasını sağlar ve değerini atar.
    /// </summary>
    public class HasUpdateDateInterceptor : IAuditPropertyInterceptor
    {
        private readonly UnitOfWorkOptions _options;
        public HasUpdateDateInterceptor(IOptions<UnitOfWorkOptions> options)
        {
            _options = options.Value;
        }
        public string PropertyName => "UpdatedDate";
        public bool Enabled { get => _options.EnableUpdatedDateAuditField; }
        public bool ShoulIntercept(Type type)
        {
            return type.GetInterfaces().Contains(typeof(IHasUpdateDate));
        }
        public void OnModelCreating(EntityTypeBuilder entityTypeBuilder)
        {
            var property = entityTypeBuilder.Metadata.GetProperties().FirstOrDefault(p => p.Name == PropertyName);
            if (property == null)
            {
                entityTypeBuilder.Property<DateTime?>(PropertyName);
            }
        }

        public void OnInsert(IUserContextProvider clientInfoProvider, DateTime operationTime, EntityEntry entityEntry)
        {
            return;
        }

        public void OnUpdate(IUserContextProvider clientInfoProvider, DateTime operationTime, EntityEntry entityEntry)
        {
            var countryCode = clientInfoProvider.CountryCode;
            switch (countryCode)
            {
                case "tr-TR":
                    entityEntry.Property(PropertyName).CurrentValue = operationTime.AddHours(3);
                    break;
                default:
                    entityEntry.Property(PropertyName).CurrentValue = operationTime;
                    break;
            }
        }

        public void OnDelete(IUserContextProvider clientInfoProvider, DateTime operationTime, EntityEntry entityEntry)
        {
            return;
        }
    }
}
