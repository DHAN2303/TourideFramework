using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Touride.Framework.Statics.Models
{
    public class Street : BaseEntity
    {
        [ForeignKey(nameof(District))]
        public Guid? StreetDistrictId { get; set; }
        public District? District { get; set; }

        [MaxLength(200)]
        [Required]
        public string? StreetName { get; set; }

        [MaxLength(250)]
        public string? StreetLatitude { get; set; }

        [MaxLength(250)]
        public string? StreetLongtitude { get; set; }

        [Required]
        public int? DisplayOrder { get; set; }

        //[Required]
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        //[Required]
        public Guid? StatusId { get; set; }

    }
}
