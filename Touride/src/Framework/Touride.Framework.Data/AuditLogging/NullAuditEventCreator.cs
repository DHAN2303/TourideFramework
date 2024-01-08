using Microsoft.EntityFrameworkCore;
using Touride.Framework.Abstractions.Client;
using Touride.Framework.Abstractions.Data.AuditLog;
using Touride.Framework.DataAudit.Common;

namespace Touride.Framework.Data.AuditLogging
{
    internal class NullAuditEventCreator : IAuditEventCreator
    {
        public Func<IEnumerable<AuditEvent>> CreateAuditEvents(DbContext dbContext, IUserContextProvider clientInfoProvider, DateTime eventTime)
        {
            return () => { return new List<AuditEvent>(); };
        }
    }
}
