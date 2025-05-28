using FluentValidation;
using SurveyBasket.Contracts.Vote;

namespace SurveyBasket.Contracts.Poll
{
    public class VoteValidate : AbstractValidator<VoteRequest>
    {
        public VoteValidate()
        {

            RuleFor(x => x.VoteAnswers)
                .NotEmpty();

            RuleForEach(V => V.VoteAnswers).SetInheritanceValidator(v => v.Add(new VoteAnswerValidate()));
            
        }















    }
}
