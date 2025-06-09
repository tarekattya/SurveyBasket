namespace SurveyBasket.Contracts.Authentication
{
    public class ResendConfirmationEmailRequestValidator:AbstractValidator<ResendConfirmationCodeRequest>
    {
        public ResendConfirmationEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
    
    
}
