using SurveyBasket.Contracts.Vote;

namespace SurveyBasket.Services.Abstractions
{
    public interface IVoteServices
    {
        Task<Result> AddAsync(int PollId, string UserId, VoteRequest request, CancellationToken cancellationToken = default);
    }
}
