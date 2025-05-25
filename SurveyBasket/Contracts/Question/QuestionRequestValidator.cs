namespace SurveyBasket.Contracts.Question
{
    public class QuestionRequestValidator : AbstractValidator<QuestionRequest> 
    {
        public QuestionRequestValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .Length(10, 1000);

            RuleFor(x => x.Answers)
                .NotEmpty()
                .WithMessage("u must add answers to Questions");

            RuleFor(x => x.Answers)
                .NotEmpty()
                .Must(x => x.Count > 1)
                .WithMessage("At least two answers are required.")
                .When(x=>x.Answers != null);

            RuleFor(A => A.Answers)
                .Must(x => x.Distinct().Count() == x.Count)
                .WithMessage("Answers must be unique for each Question")
                .When(x => x.Answers != null);
            





        }

    }
}
