﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectName.Domain.AggregatesModel.UserAggregate;

namespace ProjectName.Infrastructure.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(t => t.Id);
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        }
    }
}