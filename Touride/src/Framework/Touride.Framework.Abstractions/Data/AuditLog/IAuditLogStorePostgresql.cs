using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Touride.Framework.Abstractions.Data.AuditLog
{
    /// <summary>
    /// Audit loglarının kaydedilmesi için implementasyonu yapılacak interface
    /// </summary>
    public interface IAuditLogStorePostgresql
    {
        /// <summary>
        /// Data audit eventlerinin kaydedilmesi için kullanılır.
        /// </summary>
        /// <param name="auditEvents">Data Audit Eventleri</param>
        /// <param name="saveChanges"><see cref="DbContext.SaveChanges"/></param>
        /// <returns>SaveChages functionından etkilenen kayıt adedi.</returns>
        int StoreAuditEvents(Func<IEnumerable<AuditEvent>> auditEventsFucn, Func<int> saveChanges);
        /// <summary>
        /// Data audit eventlerinin kaydedilmesi için kullanılır.
        /// </summary>
        /// <param name="auditEvents">Data Audit Eventleri</param>
        /// <param name="saveChanges"><see cref="DbContext.SaveChangesAsync"/></param>
        /// <returns>SaveChages functionından etkilenen kayıt adedi.</returns>
        Task<int> StoreAuditEventsAsync(Func<IEnumerable<AuditEvent>> auditEventsFucn, Func<Task<int>> saveChanges);
    }
}
