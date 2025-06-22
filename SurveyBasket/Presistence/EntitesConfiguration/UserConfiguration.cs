using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MimeKit.Encodings;
using SurveyBasket.Abstractions.Const;

namespace SurveyBasket.Persistence.EntitiesConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .OwnsMany(x => x.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");

        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);

        //Default Data


        builder.HasData(new ApplicationUser
        {
            Id =DefaultUser.AdminId,
            FirstName = "Survey Basket",
            LastName = "Admin",
            UserName = DefaultUser.AdminEmail,
            NormalizedUserName = DefaultUser.AdminEmail.ToUpper(),
            Email = DefaultUser.AdminEmail,
            NormalizedEmail = DefaultUser.AdminEmail.ToUpper(),
            SecurityStamp = DefaultUser.AdminSecurity,
            ConcurrencyStamp = DefaultUser.AdminCouurency,
            EmailConfirmed = true,
            PasswordHash = "AQAAAAIAAYagAAAAEFoBZtZrBXuTETtpN2af//gQ1MSZXe55JL1W9DvO/E9344/QpahxmhAMcjCkA7Zrqg=="
        });
    }
}