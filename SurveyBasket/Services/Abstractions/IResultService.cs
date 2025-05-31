using SurveyBasket.Contracts.Results;

namespace SurveyBasket.Services.Abstractions
{
    public interface IResultService
    {
        Task<Result<PollVoteResponse>> GetResult(int PollId, CancellationToken cancellationToken = default);

        Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int PollId, CancellationToken cancellationToken = default);

        Task<Result<IEnumerable<VotesPerQuestionsResponse>>> GetVotesPerQuestionAsync(int PollId, CancellationToken cancellationToken = default);

        


    }
}
