namespace SurveyBasket.Presistence.Entites
{
    public class Vote
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;



        public Poll Poll { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

        public ICollection<VoteAnswer> VoteAnswers { get; set; } = [];

    }
}
