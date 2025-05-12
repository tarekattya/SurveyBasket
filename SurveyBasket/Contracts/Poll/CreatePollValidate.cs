using FluentValidation;

namespace SurveyBasket.Contracts.Poll
{
    public class CreatePollValidate : AbstractValidator<CreatePollRequest>
    {
        public CreatePollValidate()
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
                .WithName(nameof(CreatePollRequest.EndsAt))
                .WithMessage("{PropertyName} must be greater or equal StartsAt");





        }

        private bool BeAValidDate(CreatePollRequest poll)
        {
            return poll.EndsAt >= poll.StartsAt;
        }
    }
}
