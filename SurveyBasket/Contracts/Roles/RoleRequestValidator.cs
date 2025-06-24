namespace SurveyBasket.Contracts.Roles
{
    public class RoleRequestValidator : AbstractValidator<RoleRequest>
    {

        public RoleRequestValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty()
                .WithMessage("Role name is required.")
                .Length(3, 100);


            RuleFor(x => x.permission)
                .NotEmpty()
                .Must(x => x.Distinct().Count() == x.Count())
                .WithMessage("Permissions must be unique and cannot be empty.")
                .When(x => x.permission is not null);
        }

    }
}
