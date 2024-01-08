using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Touride.Framework.Statics.Models
{
    public class District : BaseEntity
    {
        public District()
        {
            Streets = new HashSet<Street>();
        }
        [ForeignKey(nameof(County))]
        public Guid? DistrictCountyId { get; set; }
        public County? County { get; set; }

        [Required]
        [MaxLength(50)]
        public string DistrictName { get; set; }
        [MaxLength(250)]
        public string? DistrictLatitude { get; set; }
        [MaxLength(250)]
        public string? DistrictLongtitude { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public Guid StatusId { get; set; }

        public virtual ICollection<Street> Streets { get; set; }
    }
}
