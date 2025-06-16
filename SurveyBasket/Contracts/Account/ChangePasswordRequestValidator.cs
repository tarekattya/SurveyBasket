namespace SurveyBasket.Contracts.Account
{
    public class ChangePasswordRequestValidator: AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.oldpassword)
                .NotEmpty()
                .WithMessage("Old password is required.");
            RuleFor(x => x.newpassword)
                .NotEmpty()
                .WithMessage("New password is required.")
                .NotEqual(x=>x.oldpassword)
                .WithMessage("New password must be different from old password.")
                .Matches(SurveyBasket.Abstractions.Regex.Pattern.PasswordPattern)
                .WithMessage("New password must have upper case letter, lower case letter, number, and special character.");
            RuleFor(x => x.confirmpassword)
                .NotEmpty()
                .WithMessage("Confirm password is required.")
                .Equal(x => x.newpassword)
                .WithMessage("Confirm password must match the new password.");

        }
    }
}
