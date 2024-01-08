using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Touride.Framework.Statics.Models
{
    public class Region : BaseEntity
    {
        public Region()
        {
            Languages = new HashSet<Language>();
            RegionalSettings = new HashSet<RegionalSetting>();
            RegionCountryMappings = new HashSet<RegionCountryMapping>();
        }
        [Required]
        public string RegionName { get; set; }
        [Required]
        public string RegionCode { get; set; }
        public string? RegionLatitude { get; set; }
        public string? RegionLongitude { get; set; }

        [ForeignKey(nameof(Country))]
        public Guid? CountryId { get; set; }
        public Country? Country { get; set; }

        [MaxLength(50)]
        public string? IndexName { get; set; }

        [MaxLength(100)]
        public string? Domain { get; set; }

        [MaxLength(100)]
        public string? RegionFlagImageFileName { get; set; }

        [Required]
        public int AvailableInStore { get; set; }

        [MaxLength(100)]
        public string? LocaleName { get; set; }

        public int? OldId { get; set; }
        public virtual ICollection<Language> Languages { get; set; }
        public virtual ICollection<RegionalSetting> RegionalSettings { get; set; }
        public virtual ICollection<RegionCountryMapping> RegionCountryMappings { get; set; }
    }
}
