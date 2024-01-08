namespace Order.Application.Models
{
    public class AddressDto
    {
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }

        public AddressDto() : this(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty)
        {
        }

        public AddressDto(
            string street,
            string city,
            string state,
            string country)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
        }
    }
}