using SurveyBasket.Contracts.Vote;
using SurveyBasket.Presistence.DbContextt;

namespace SurveyBasket.Services
{
    public class VoteServices(ApplicationDbContext context) : IVoteServices
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> AddAsync(int PollId, string UserId, VoteRequest request, CancellationToken cancellationToken = default)
        {
            var hasVoted = await _context.Votes.AnyAsync(V => V.PollId == PollId && V.UserId == UserId, cancellationToken);
            if(hasVoted)
                return Result.Failure(VoteErrors.DublicateVotes);

            var pollIsExist = await _context.Polls
                .AnyAsync(P => P.Id == PollId && P.IsPublished && P.StartsAt < DateTime.UtcNow && P.EndsAt > DateTime.UtcNow, cancellationToken);

            if(!pollIsExist)
                return Result.Failure(PollErrors.PollNotFound);

            var avaliableQuestion = await _context.Questions
                .Where(Q => Q.IsActive && Q.PollId == PollId)
                .Select(Q => Q.Id)
                .ToListAsync(cancellationToken);

            if (!request.VoteAnswers.Select(VA => VA.QuestionId).SequenceEqual(avaliableQuestion))
                return Result.Failure(VoteErrors.InvalidQuestions);

            var vote = new Vote
            {
                PollId = PollId,
                UserId = UserId,
                VoteAnswers = request.VoteAnswers.Adapt<IEnumerable<VoteAnswer>>().ToList()


            };

           await _context.AddAsync(vote , cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }
    }
}
