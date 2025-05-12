
namespace SurveyBasket.Contracts.Authentication
{
    public class AuthRequestValidate : AbstractValidator<AuthRequst>
    {
        public AuthRequestValidate()
        {



            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty();








        }

       

    }
}
