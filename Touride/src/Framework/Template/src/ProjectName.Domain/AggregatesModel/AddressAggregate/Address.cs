using ProjectName.Domain.AggregatesModel.UserAggregate;

namespace ProjectName.Domain.AggregatesModel.AddressAggregate
{
    public class Address : BaseEntitiy
    {

        public string? Name { get; set; }

        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
