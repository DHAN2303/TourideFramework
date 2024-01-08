using System.ComponentModel.DataAnnotations;

namespace Touride.Framework.Statics.Models
{
    public class DynamicStringResource : BaseEntity
    {
        public int? Entity2Id { get; set; }
        public Guid? EntityId { get; set; }
        [Required]
        public int LanguageIndex { get; set; }
        [Required]
        [MaxLength(200)]
        public string ModelName { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
