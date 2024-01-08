using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Reflection;
using Touride.Framework.Abstractions.Client;
using Touride.Framework.Abstractions.Data.AuditLog;

namespace Touride.Framework.DataAudit.Common
{
    public class AuditEventCreator : IAuditEventCreator
    {

        private static readonly ConcurrentDictionary<Type, bool> EntityAuditableState = new ConcurrentDictionary<Type, bool>();
        private static readonly ConcurrentDictionary<Type, HashSet<string>> IgnoredTypeProperties = new ConcurrentDictionary<Type, HashSet<string>>();
        private List<AuditEntityEntry> TrackingEntities = new List<AuditEntityEntry>();
        public Func<IEnumerable<AuditEvent>> CreateAuditEvents(DbContext dbContext, IUserContextProvider clientInfoProvider, DateTime eventTime)
        {
            SetTrackedEntities(dbContext);
            return new Func<IEnumerable<AuditEvent>>(() => GetAuditEvents(dbContext, clientInfoProvider, eventTime));
        }

        private IEnumerable<AuditEvent> GetAuditEvents(DbContext dbContext, IUserContextProvider clientInfoProvider, DateTime eventTime)
        {
            var events = new List<AuditEvent>();
            var modifiedEntries = GetTrackedEntities();
            if (modifiedEntries.Count == 0)
            {
                return events;
            }
            foreach (var entry in modifiedEntries)
            {
                var primaryKeyProperties = entry.EntityEntry.Metadata.FindPrimaryKey()?.Properties;
                var entityMetaData = GetEntityMetaData(dbContext, entry.EntityEntry);

                events.Add(new AuditEvent()
                {
                    Id = Guid.NewGuid(),
                    EventTime = eventTime,
                    EventType = entry.EventType,
                    PkValue1 = primaryKeyProperties != null ? entry.EntityEntry.CurrentValues.GetValue<Guid>(primaryKeyProperties[0]) : Guid.Empty,
                    PkValue2 = primaryKeyProperties != null && primaryKeyProperties.Count > 1 ? entry.EntityEntry.CurrentValues.GetValue<Guid>(primaryKeyProperties[1]) : Guid.Empty,
                    DatabaseName = entityMetaData.DatabaseName,
                    SchemaName = entityMetaData.SchemaName,
                    TableName = entityMetaData.TableName,
                    User = clientInfoProvider.UserId,
                    PropertyValues = GetAuditablePropertyValues(dbContext, entry.EntityEntry),
                    CorrelationId = clientInfoProvider.CorrelationId,
                    CorrelationSeq = clientInfoProvider.CorrelationSeq
                });
            }
            ClearTrackedEntities();
            return events;
        }

        private void SetTrackedEntities(DbContext dbContext)
        {
            dbContext.ChangeTracker.DetectChanges();
            TrackingEntities.AddRange(
                dbContext.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged
                         && x.State != EntityState.Detached
                         && IsEntityAuditable(x.Entity))
                .Select((entityEntry) => new AuditEntityEntry() { EntityEntry = entityEntry, EventType = EntityStateToAuditEvent(entityEntry.State) }).ToList()
            );
        }

        private List<AuditEntityEntry> GetTrackedEntities()
        {
            return TrackingEntities;
        }

        private void ClearTrackedEntities()
        {
            TrackingEntities.Clear();
        }

        private static bool IsEntityAuditable(object entity)
        {
            var type = entity.GetType();
            if (!EntityAuditableState.ContainsKey(type))
            {
                var ignoreAttribute = type.GetTypeInfo().GetCustomAttribute(typeof(AuditLogIgnoreAttribute), true);
                EntityAuditableState[type] = ignoreAttribute != null ? false : true;
            }
            return EntityAuditableState[type];
        }

        private static string EntityStateToAuditEvent(EntityState entityState)
        {
            switch (entityState)
            {
                case EntityState.Added:
                    return AuditEventType.Insert;
                case EntityState.Modified:
                    return AuditEventType.Update;
                case EntityState.Deleted:
                    return AuditEventType.Delete;
                default:
                    return AuditEventType.Unknown;
            }
        }

        private static EntityMetaData GetEntityMetaData(DbContext dbContext, EntityEntry entityEntry)
        {
            var result = new EntityMetaData();
            var definingType = dbContext.Model.FindEntityType(entityEntry.Entity.GetType());
            if (definingType == null)
            {
                return result;
            }
            var dbConnection = IsRelational(dbContext) ? dbContext.Database.GetDbConnection() : null;
            result.DatabaseName = dbConnection?.Database;
            result.TableName = definingType.GetTableName();
            result.SchemaName = definingType.GetSchema();
            return result;
        }

        private static Dictionary<string, object> GetAuditablePropertyValues(DbContext dbContext, EntityEntry entityEntry)
        {
            var result = new Dictionary<string, object>();
            var properties = entityEntry.Metadata.GetProperties();
            foreach (var prop in properties)
            {
                PropertyEntry propEntry = entityEntry.Property(prop.Name);
                if (IsPropertyAuditable(dbContext, entityEntry, prop.Name))
                {
                    result.Add(prop.GetColumnName(), propEntry.CurrentValue);
                }
            }
            return result;
        }

        private static bool IsPropertyAuditable(DbContext dbContext, EntityEntry entityEntry, string propertyName)
        {
            var entityType = entityEntry.Metadata.ClrType;
            if (entityType == null)
            {
                return true;
            }
            var ignoredProperties = GetAuditIgnoredPropertiesOfType(entityType);
            if (ignoredProperties != null && ignoredProperties.Contains(propertyName))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static HashSet<string> GetAuditIgnoredPropertiesOfType(Type type)
        {
            if (!IgnoredTypeProperties.ContainsKey(type))
            {
                var ignoredProps = new HashSet<string>();
                foreach (var prop in type.GetTypeInfo().GetProperties())
                {
                    var ignoreAttr = prop.GetCustomAttribute(typeof(AuditLogIgnorePropertyAttribute), true);
                    if (ignoreAttr != null)
                    {
                        ignoredProps.Add(prop.Name);
                    }
                }
                if (ignoredProps.Count > 0)
                {
                    IgnoredTypeProperties[type] = ignoredProps;
                }
                else
                {
                    IgnoredTypeProperties[type] = null;
                }
            }
            return IgnoredTypeProperties[type];
        }

        private static bool IsRelational(DbContext dbContext)
        {
            var provider = (IInfrastructure<IServiceProvider>)dbContext.Database;
            var relationalConnection = provider.Instance.GetService<IRelationalConnection>();
            return relationalConnection != null;
        }

    }
}
