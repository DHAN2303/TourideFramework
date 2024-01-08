using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Touride.Framework.Statics.Models
{
    public class Country : BaseEntity
    {
        public Country()
        {
            Cities = new HashSet<City>();
            CityLanguageMappings = new HashSet<CityLanguageMapping>();
            CountryRegions = new HashSet<CountryRegion>();
            Regions = new HashSet<Region>();
            RegionCountryMappings = new HashSet<RegionCountryMapping>();
        }

        //[Required]
        [ForeignKey(nameof(Language))]
        public Guid? CountryLanguageId { get; set; }
        public Language? Language { get; set; }

        //[Required]
        public string? CountryName { get; set; }

        [MaxLength(4)]
        public string? CountryCode { get; set; }
        public string? CountryRegexPostalCode { get; set; }
        public string? CountryRegexMobilePhone { get; set; }

        //[Required]
        public int? DisplayOrder { get; set; }

        // [Required]
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        //[Required]
        public Guid? StatusId { get; set; }

        //[Required]
        public int? CountryLanguageIndex { get; set; }
        public string? CountryLatitude { get; set; }
        public string? CountryLongitude { get; set; }

        [MaxLength(100)]
        public string? CountryFlagImageFileName { get; set; }
        public int? CountryStoreStockBuffer { get; set; }
        public bool? UseCountryStoreStockBuffer { get; set; }

        [MaxLength(10)]
        public string? CountryPhoneCodePrefix { get; set; }

        //[Required]
        public int? CountryCodeIsoNumber { get; set; }

        public virtual ICollection<City>? Cities { get; set; }
        public virtual ICollection<CityLanguageMapping> CityLanguageMappings { get; set; }
        public virtual ICollection<CountryRegion> CountryRegions { get; set; }
        public virtual ICollection<Region> Regions { get; set; }
        public virtual ICollection<RegionCountryMapping> RegionCountryMappings { get; set; }
    }
}
