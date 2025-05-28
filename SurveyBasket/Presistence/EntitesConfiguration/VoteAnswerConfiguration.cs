using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurveyBasket.Presistence.EntitesConfiguration
{
    public class VoteAnswerConfiguration : IEntityTypeConfiguration<VoteAnswer>
    {
        public void Configure(EntityTypeBuilder<VoteAnswer> builder)
        {
            builder.HasIndex(A => new {A.VoteId , A.QuestionId})
                .IsUnique();

        }
    }
}
