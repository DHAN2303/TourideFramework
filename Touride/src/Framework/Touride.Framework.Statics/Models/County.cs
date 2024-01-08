using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Touride.Framework.Statics.Models
{
    public class County : BaseEntity
    {
        public County()
        {
            CountyLanguageMappings = new HashSet<CountyLanguageMapping>();
            Districts = new HashSet<District>();
        }

        //[Required]
        [ForeignKey(nameof(City))]
        public Guid? CountyCityId { get; set; }
        public City? City { get; set; }

        [Required]
        [MaxLength(50)]
        public string CountyName { get; set; }

        [MaxLength(250)]
        public string? CountyLatitude { get; set; }

        [MaxLength(250)]
        public string? CountyLongtitude { get; set; }

        [Required]
        public int DisplayOrder { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [MaxLength(50)]
        public string? CountyInOmsName { get; set; }

        [Required]
        public int CountyCode { get; set; }

        public virtual ICollection<CountyLanguageMapping> CountyLanguageMappings { get; set; }
        public virtual ICollection<District> Districts { get; set; }
    }
}
