using SurveyBasket.Abstractions.Regex;

namespace SurveyBasket.Contracts.User
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(3, 100)
                .WithMessage("First name is required.");
            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(3, 100)
                .WithMessage("Last name is required.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid email is required.");
           
            RuleFor(x => x.Roles)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Roles)
                .Must(x => x.Distinct().Count() == x.Count())
                .WithMessage("Roles must be unique to each user.")
                .When(x => x.Roles != null);

        }
    }
}
