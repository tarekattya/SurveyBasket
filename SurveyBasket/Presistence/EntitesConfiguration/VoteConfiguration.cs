using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurveyBasket.Presistence.EntitesConfiguration
{
    public class VoteConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.HasIndex(A => new {A.PollId , A.UserId})
                .IsUnique();

        }
    }
}
