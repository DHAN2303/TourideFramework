using Touride.Domain.AggregatesModel.UserAggregate;

namespace Touride.Domain.AggregatesModel.AddressAggregate
{
    public class Address : BaseEntitiy
    {

        public string? Name { get; set; }

        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
