﻿namespace SurveyBasket.Contracts.Results
{
    public record VoteResponse(
        
        string VoterName,
        DateTime VoteDate,
        IEnumerable<QuesttionAnswerResponse> SelectedAnswers
        
        
        );
    
}
