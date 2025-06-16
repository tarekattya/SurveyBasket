namespace SurveyBasket.Presistence.Entites
{
    public class Poll : AuditableEntity
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;

        public bool IsPublished { get; set; }

        public DateOnly StartsAt { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public DateOnly EndsAt { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);


        public ICollection<Question> Questions { get; set; } = [];
        public ICollection<Vote> Votes { get; set; } = [];


    }



}

