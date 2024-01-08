using Touride.Framework.Abstractions.Data.AuditLog;

namespace Touride.Framework.Data.AuditLogging
{
    internal class NullAuditLogStore : IAuditLogStore
    {
        public int StoreAuditEvents(Func<IEnumerable<AuditEvent>> auditEventsFucn, Func<int> saveChanges)
        {
            return saveChanges();
        }

        public async Task<int> StoreAuditEventsAsync(Func<IEnumerable<AuditEvent>> auditEventsFucn, Func<Task<int>> saveChanges)
        {
            return await saveChanges();
        }
    }
}
