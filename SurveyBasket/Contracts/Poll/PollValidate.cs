using FluentValidation;

namespace SurveyBasket.Contracts.Poll
{
    public class PollValidate : AbstractValidator<PollRequest>
    {
        public PollValidate()
        {


            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("u must add a {PropertyName}")
                .Length(3, 115)
                .WithMessage("the Minimum of chars is {MinLength} and Maximum is {MaxLength} you enterd {TotalLength}");
            ;

            RuleFor(x => x.Summary)
                .NotEmpty()
                .WithMessage("u must add a {PropertyName}")
                .Length(3, 1500)
                .WithMessage("the Minimum of chars is {MinLength} and Maximum is {MaxLength} you enterd {TotalLength}");

            RuleFor(x => x.StartsAt)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Today);

            RuleFor(x => x.EndsAt)
                .NotEmpty();

            RuleFor(x => x)
                .Must(BeAValidDate)
                .WithName(nameof(PollRequest.EndsAt))
                .WithMessage("{PropertyName} must be greater or equal StartsAt");





        }

        private bool BeAValidDate(PollRequest poll)
        {
            return poll.EndsAt >= poll.StartsAt;
        }
    }
}
