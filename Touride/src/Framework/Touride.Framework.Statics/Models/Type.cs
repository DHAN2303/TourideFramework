using System.ComponentModel.DataAnnotations;

namespace Touride.Framework.Statics.Models
{
    public class Type : BaseEntity
    {
        public Type()
        {
            LocaleStringResources = new HashSet<LocaleStringResource>();
        }

        [MaxLength(6)]
        [Required]
        public string Code { get; set; }
        public Guid? ParentTypeId { get; set; }

        [MaxLength(100)]
        [Required]
        public string Description { get; set; }

        [MaxLength(50)]
        public string? StrCode { get; set; }

        [MaxLength(250)]
        public string? InOmsName { get; set; }
        public int? DisplayOrder { get; set; }

        public virtual ICollection<LocaleStringResource> LocaleStringResources { get; set; }
    }
}
