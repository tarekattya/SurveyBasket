namespace SurveyBasket.Presistence.Entites
{
    public class Poll : AuditableEntity
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;

        public bool IsPublished { get; set; }

        public DateTime StartsAt { get; set; } = DateTime.UtcNow;

        public DateTime EndsAt { get; set; } = DateTime.UtcNow;


    }



}

