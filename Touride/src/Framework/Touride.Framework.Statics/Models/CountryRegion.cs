using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Touride.Framework.Statics.Models
{
    public class CountryRegion : BaseEntity
    {
        //public CountryRegion()
        //{
        //    Cities = new HashSet<City>();
        //}

        //[Required]
        [ForeignKey("Country")]
        public Guid? RegionCountryId { get; set; }
        public Country? Country { get; set; }

        //[Required]
        [MaxLength(50)]
        public string? RegionName { get; set; }

        //[Required]
        public int? DisplayOrder { get; set; }

        //[Required]
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        //[Required]
        public Guid? StatusId { get; set; }

        [MaxLength(50)]
        public string? RegionInOmsName { get; set; }

        //[Required]
        public int? RegionNumber { get; set; }

        //public virtual ICollection<City> Cities { get; set; }
    }
}
