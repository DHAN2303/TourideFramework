using Touride.Framework.Abstractions.Data.AuditProperties;
using Touride.Framework.Abstractions.Data.Entities;

namespace Touride.Framework.Data.Entities
{
    public abstract class FullAuditedEntity<T> : Entity<T>, IHasFullAudit, IHasId
    {
        public Guid Id { get; set; }
    }
}