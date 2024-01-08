using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Touride.Framework.Statics.Models
{
    public class City : BaseEntity
    {
        public City()
        {
            CityLanguageMappings = new HashSet<CityLanguageMapping>();
            Counties = new HashSet<County>();
            CountyLanguageMappings = new HashSet<CountyLanguageMapping>();
        }

        //[Required]
        [ForeignKey(nameof(Country))]
        public Guid? CityCountryId { get; set; }
        public Country? Country { get; set; }

        //[Required]
        public int? CityCode { get; set; }

        //[Required]
        public string? CityName { get; set; }

        [MaxLength(250)]
        public string? CityLatitude { get; set; }

        [MaxLength(250)]
        public string? CityLongtitude { get; set; }

        //[Required]
        public int? DisplayOrder { get; set; }

        //[Required]
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        //[Required]
        public Guid? StatusId { get; set; }

        //[ForeignKey(nameof(CountryRegion))]
        //public Guid? CityCountryRegionId { get; set; }
        //public CountryRegion? CountryRegion { get; set; }

        [MaxLength(50)]
        public string? CityInOmsName { get; set; }

        //[Required]
        public int? CityNumber { get; set; }

        public virtual ICollection<CityLanguageMapping>? CityLanguageMappings { get; set; }
        public virtual ICollection<County>? Counties { get; set; }
        public virtual ICollection<CountyLanguageMapping>? CountyLanguageMappings { get; set; }
    }
}
