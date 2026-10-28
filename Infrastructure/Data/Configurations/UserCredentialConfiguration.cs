using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class UserCredentialConfiguration
    : IEntityTypeConfiguration<UserCredential>
    {
        public void Configure(
            EntityTypeBuilder<UserCredential> builder)
        {
            builder.ToTable("user_credentials");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Username)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.PasswordHash)
                .HasMaxLength(500)
                .IsRequired();

            builder.HasIndex(x => x.Username)
                .IsUnique();

            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<UserCredential>(
                    x => x.UserId);
        }
    }
}
