using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectName.Domain.AggregatesModel.AddressAggregate;

namespace ProjectName.Infrastructure.Configurations
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
