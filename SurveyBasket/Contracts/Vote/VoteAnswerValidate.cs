using FluentValidation;
using SurveyBasket.Contracts.Vote;

namespace SurveyBasket.Contracts.Poll
{
    public class VoteAnswerValidate : AbstractValidator<VoteAnswerRequest>
    {
        public VoteAnswerValidate()
        {

            RuleFor(x => x.AnswerId)
                .GreaterThan(0);

            RuleFor(x => x.QuestionId)
               .GreaterThan(0);

        }















    }
}
