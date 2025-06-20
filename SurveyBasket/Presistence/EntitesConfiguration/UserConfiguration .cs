﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurveyBasket.Presistence.EntitesConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            builder.OwnsMany(x=>x.RefreshTokens)
                .ToTable("RefreshTokens")
                .WithOwner()
                .HasForeignKey("UserId");

            builder.Property(p => p.FirstName).HasMaxLength(50);
            builder.Property(p => p.LastName).HasMaxLength(50);

            builder.Property(u => u.ProfileImage)
                         .HasColumnType("varbinary(max)")
                         .IsRequired(false); // الصورة اختيارية

            builder.Property(u => u.ProfileImageContentType)
                   .HasMaxLength(100)
                   .IsRequired(false);
        }
    }
}
