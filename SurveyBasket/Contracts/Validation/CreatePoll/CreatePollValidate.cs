using FluentValidation;
using SurveyBasket.Contracts.Requests;

namespace SurveyBasket.Contracts.Validation.CreatePoll
{
    public class CreatePollValidate : AbstractValidator<CreatePollRequest>
    {
        public CreatePollValidate() {


            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("u must add a {PropertyName}")
                .Length(3,115)
                .WithMessage("the Minimum of chars is {MinLength} and Maximum is {MaxLength} you enterd {TotalLength}");
                ;

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("u must add a {PropertyName}")
                .Length(5,150)
                .WithMessage("the Minimum of chars is {MinLength} and Maximum is {MaxLength} you enterd {TotalLength}");






        }
    }
}
