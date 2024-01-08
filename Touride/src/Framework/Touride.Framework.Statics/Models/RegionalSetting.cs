using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Touride.Framework.Statics.Models
{
    public class RegionalSetting : BaseEntity
    {
        //[Required]
        [ForeignKey(nameof(Region))]
        public Guid? RegionId { get; set; }
        public Region? Region { get; set; }

        [Required]
        public Guid RegionalSettingType { get; set; }

        [Required]
        public string Value { get; set; }

        //[Required]
        //[MaxLength(400)]
        public string? ApplicationTypes { get; set; }
    }
}
