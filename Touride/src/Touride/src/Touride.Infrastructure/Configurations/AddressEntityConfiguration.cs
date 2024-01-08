using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Touride.Domain.AggregatesModel.AddressAggregate;

namespace Touride.Infrastructure.Configurations
{
    public class AddressEntityConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");
            builder.HasKey(t => t.Id);
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        }
    }
}
