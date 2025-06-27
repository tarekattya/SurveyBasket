using SurveyBasket.Abstractions.Regex;

namespace SurveyBasket.Contracts.User
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
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
            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(Pattern.PasswordPattern)
                .WithMessage("Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");

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
