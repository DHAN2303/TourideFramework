using System.ComponentModel.DataAnnotations;
using Touride.Framework.Abstractions.Data.AuditProperties;
using Touride.Framework.Abstractions.Data.Entities;

namespace Touride.Domain.AggregatesModel
{
    public class BaseEntitiy : IHasId, IHasFullAudit
    {
        [Key]
        public Guid Id { get; set; }
        public bool isDeleted { get; set; } = false;
        public bool isActive { get; set; }
    }
}
