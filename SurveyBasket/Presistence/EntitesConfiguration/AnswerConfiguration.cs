using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurveyBasket.Presistence.EntitesConfiguration
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasIndex(A => new {A.QuestionId , A.Content})
                .IsUnique();
            builder.Property(p => p.Content).HasMaxLength(1000);

        }
    }
}
