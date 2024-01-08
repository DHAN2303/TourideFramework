using System.ComponentModel.DataAnnotations.Schema;

namespace Touride.Framework.Statics.Models
{
    public class RegionCountryMapping : BaseEntity
    {
        [ForeignKey(nameof(Region))]
        public Guid? RegionId { get; set; }
        public Region? Region { get; set; }

        [ForeignKey(nameof(Country))]
        public Guid? CountryId { get; set; }
        public Country? Country { get; set; }
        public int? MainRegionId { get; set; } //TODO: Sor
    }
}
