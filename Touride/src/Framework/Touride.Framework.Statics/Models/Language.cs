using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Touride.Framework.Statics.Models
{
    public class Language : BaseEntity
    {
        public Language()
        {
            CityLanguageMappings = new HashSet<CityLanguageMapping>();
            Countries = new HashSet<Country>();
            CountyLanguageMappings = new HashSet<CountyLanguageMapping>();
            LocaleStringResources = new HashSet<LocaleStringResource>();
        }

        [Required]
        public Guid LanguageStatusId { get; set; }
        [Required]
        [MaxLength(50)]
        public string LanguageName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LanguageCulture { get; set; }
        [Required]
        [MaxLength(50)]
        public string LanguageFlagImageFileName { get; set; }
        public int? LanguageDisplayOrder { get; set; }
        [Required]
        public bool LanguageIsRtl { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        public Guid StatusId { get; set; }
        [Required]
        public int LanguageIndex { get; set; }
        //[Required]
        [ForeignKey(nameof(Region))]
        public Guid? LanguageRegionId { get; set; }
        public Region? Region { get; set; }
        public int? LanguageParentId { get; set; } // TODO: Sor

        public int? OldLanguageRegionId { get; set; }

        [Required]
        public bool LanguageIsDefault { get; set; }
        [MaxLength(10)]
        public string? Currency { get; set; }
        [MaxLength(10)]
        public string? CurrencySymbol { get; set; }
        [MaxLength(50)]
        public string? ElasticSearchIndexName { get; set; }
        [MaxLength(50)]
        public string? CurrencyDisplay { get; set; }
        [Required]
        public int CurrencyId { get; set; }
        [Required]
        public bool LanguageIsGlobalDefault { get; set; }
        [Required]
        public bool LanguageIsSeoLanguage { get; set; }
        [Required]
        public bool IsRightToLeft { get; set; }

        public virtual ICollection<CityLanguageMapping> CityLanguageMappings { get; set; }
        public virtual ICollection<Country> Countries { get; set; }
        public virtual ICollection<CountyLanguageMapping> CountyLanguageMappings { get; set; }
        public virtual ICollection<LocaleStringResource> LocaleStringResources { get; set; }
    }
}
