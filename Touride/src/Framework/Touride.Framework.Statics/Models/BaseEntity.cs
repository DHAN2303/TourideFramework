using System.ComponentModel.DataAnnotations;
using Touride.Framework.Abstractions.Data.AuditProperties;
using Touride.Framework.Abstractions.Data.Entities;

namespace Touride.Framework.Statics.Models
{
    public class BaseEntity : IHasId, IHasFullAudit
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
    }
}
