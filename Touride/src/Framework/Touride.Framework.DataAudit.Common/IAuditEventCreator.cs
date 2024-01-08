using Microsoft.EntityFrameworkCore;
using Touride.Framework.Abstractions.Client;
using Touride.Framework.Abstractions.Data.AuditLog;

namespace Touride.Framework.DataAudit.Common
{
    public interface IAuditEventCreator
    {
        Func<IEnumerable<AuditEvent>> CreateAuditEvents(DbContext dbContext, IUserContextProvider clientInfoProvider, DateTime eventTime);
    }
}