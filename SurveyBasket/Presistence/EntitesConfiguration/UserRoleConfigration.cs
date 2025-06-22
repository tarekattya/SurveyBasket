using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurveyBasket.Presistence.EntitesConfiguration
{
    public class UserRoleConfigration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(

                    new IdentityUserRole<string>
                    {
                        UserId = DefaultUser.AdminId,
                        RoleId = RoleDefault.AdminRoleId
                    }
                );

        }
    }
}
