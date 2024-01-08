using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Touride.Framework.Abstractions.Data.AuditProperties;
using Touride.Framework.Abstractions.Data.Entities;

namespace Touride.Framework.Data.Entities
{
    public abstract class BaseFullAudit : IHasId, IHasFullAudit
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string CreatedAt { get; set; }
        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string CreatedBy { get; set; }
        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string CreatedByUserCode { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string UpdatedAt { get; set; }
        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string UpdatedBy { get; set; }
        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string UpdatedByUserCode { get; set; }
    }
}
