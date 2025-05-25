namespace SurveyBasket.Contracts.Question
{
    public record QuestionResponse(
        int id,
        string Content,
        IEnumerable<AnswerResponse> Answers


        );
    
    
}
