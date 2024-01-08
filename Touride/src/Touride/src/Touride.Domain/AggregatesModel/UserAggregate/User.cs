using Touride.Domain.AggregatesModel.AddressAggregate;

namespace Touride.Domain.AggregatesModel.UserAggregate
{
    public class User : BaseEntitiy
    {
        public User()
        {
            Addresses = new HashSet<Address>();
        }


        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }



        public virtual ICollection<Address> Addresses { get; set; }
    }
}
