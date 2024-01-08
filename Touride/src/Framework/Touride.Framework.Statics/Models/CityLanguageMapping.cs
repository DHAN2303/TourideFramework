using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Touride.Framework.Statics.Models
{
    public class CityLanguageMapping : BaseEntity
    {
        //[Required]
        [ForeignKey(nameof(Country))]
        public Guid? CityCountryId { get; set; }
        public Country? Country { get; set; }

        //[Required]
        [ForeignKey(nameof(City))]
        public Guid? CityId { get; set; }
        public City? City { get; set; }

        //[Required]
        [ForeignKey(nameof(Language))]
        public Guid? LanguageId { get; set; }
        public Language? Language { get; set; }

        //[Required]
        [MaxLength(150)]
        public string? CityName { get; set; }
    }
}
