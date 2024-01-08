using Touride.Abstraction.Dtos;

namespace Touride.Abstraction.Models.TestModels
{
    public class CreateTestModel
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public bool isDeleted { get; set; }

        public List<AddressDto>? Addresses { get; set; }
    }
}
