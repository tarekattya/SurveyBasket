namespace SurveyBasket.Contracts.Results
{
    public record VotesPerQuestionsResponse(
        
        string Content,
        IEnumerable<QuestionPerAnswers> SelectedAnswers
        
        );
    
    
}
