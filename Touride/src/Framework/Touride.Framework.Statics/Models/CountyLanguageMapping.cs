using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Touride.Framework.Statics.Models
{
    public class CountyLanguageMapping : BaseEntity
    {
        //[Required]
        [ForeignKey(nameof(City))]
        public Guid? CountyCityId { get; set; }
        public City? City { get; set; }

        //[Required]
        [ForeignKey(nameof(County))]
        public Guid? CountyId { get; set; }
        public County? County { get; set; }

        //[Required]
        [ForeignKey(nameof(Language))]
        public Guid? LanguageId { get; set; }
        public Language? Language { get; set; }

        //[Required]
        [MaxLength(50)]
        public string? CountyName { get; set; }
    }
}
