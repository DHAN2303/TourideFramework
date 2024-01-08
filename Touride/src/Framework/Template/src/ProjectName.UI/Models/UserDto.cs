using Touride.Framework.Abstractions.Data.Entities;

namespace ProjectName.UI.Models
{
    public class UserDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public bool isDeleted { get; set; } = false;

        public virtual ICollection<AddressDto> Addresses { get; set; }
    }
}
