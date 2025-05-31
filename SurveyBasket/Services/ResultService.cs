using SurveyBasket.Contracts.Results;
using SurveyBasket.Presistence.DbContextt;

namespace SurveyBasket.Services
{
    public class ResultService(ApplicationDbContext context) : IResultService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<PollVoteResponse>> GetResult(int PollId , CancellationToken cancellationToken = default)
        { 
            var VotesResult = await _context.Polls
                .Where(p => p.Id == PollId)
                .Select(x => new PollVoteResponse(

                    x.Title,
                    x.Summary,
                    x.StartsAt,
                    x.EndsAt,
                    x.Votes.Select(v => new VoteResponse(
                        $"{v.User.FirstName} {v.User.LastName}",
                        v.SubmittedOn,
                        v.VoteAnswers.Select(a => new QuesttionAnswerResponse(
                            a.Question.Content,
                            a.Answer.Content
                            ))
                        ))
                    ))
                .SingleOrDefaultAsync(cancellationToken);
            return VotesResult is null ?
                Result.Failure<PollVoteResponse>(PollErrors.PollNotFound) :
                Result.Success(VotesResult);
        }


        public async Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int PollId , CancellationToken cancellationToken = default)
        {

            var PollIsExist = await _context.Polls.AnyAsync(P => P.Id == PollId);

            if (!PollIsExist)
                return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollErrors.PollNotFound);

            var VotesPerDay = await _context.Votes
                .Where(V => V.PollId == PollId)
                .GroupBy(V => new { Date = DateOnly.FromDateTime(V.SubmittedOn)})
                .Select(V => new VotesPerDayResponse(
                    
                    V.Key.Date,
                    V.Count() 
                    ))
                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<VotesPerDayResponse>>(VotesPerDay);


        }



        public async Task<Result<IEnumerable<VotesPerQuestionsResponse>>> GetVotesPerQuestionAsync(int PollId, CancellationToken cancellationToken = default)
        {

            var PollIsExist = await _context.Polls.AnyAsync(P => P.Id == PollId);

            if (!PollIsExist)
                return Result.Failure<IEnumerable<VotesPerQuestionsResponse>>(PollErrors.PollNotFound);

            var VotesPerQuestion = await _context.VoteAnswers
                .Where(V => V.Vote.PollId == PollId)
                .Select(V => new VotesPerQuestionsResponse(

                    V.Question.Content,
                    V.Question.Votes
                    .GroupBy(Q => new { AnswerId = Q.Answer.Id, AnswerContent = Q.Answer.Content})
                    .Select(G => new QuestionPerAnswers(
                        
                        G.Key.AnswerContent,
                        G.Count()
                        
                        ))

                    ))
                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<VotesPerQuestionsResponse>>(VotesPerQuestion);


        }

    }
}
