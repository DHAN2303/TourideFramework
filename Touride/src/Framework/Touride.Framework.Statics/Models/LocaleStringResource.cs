using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Touride.Framework.Statics.Models
{
    public class LocaleStringResource : BaseEntity
    {

        [ForeignKey(nameof(Language))]
        //[Required]
        public Guid? ResourceLanguageId { get; set; }
        public Language? Language { get; set; }

        [Required]
        [MaxLength(200)]
        public string ResourceName { get; set; }

        //[Required]
        public string? ResourceValue { get; set; }
        public int? ResourceLanguageIndex { get; set; }

        //[Required]
        [ForeignKey("Type")]
        public Guid? ApplicationTypeId { get; set; }
        public Touride.Framework.Statics.Models.Type? Type { get; set; }

    }
}
