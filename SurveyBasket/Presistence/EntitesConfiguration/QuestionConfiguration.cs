using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurveyBasket.Presistence.EntitesConfiguration
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasIndex(Q => new { Q.PollId , Q.Content })
                .IsUnique();
            builder.Property(Q => Q.Content).HasMaxLength(1000);

        }
    }
}
