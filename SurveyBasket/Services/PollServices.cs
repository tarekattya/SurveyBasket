using Microsoft.EntityFrameworkCore;
using SurveyBasket.Presistence.DbContextt;
using SurveyBasket.Services.NewFolder;

namespace SurveyBasket.Services
{
    public class PollServices(ApplicationDbContext context) : IPollServices
    {
        private readonly ApplicationDbContext _context = context;



        public async Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken) 
        { 
            
            
           var polls = await _context.Polls.AsNoTracking().ToListAsync();
            var result = polls.Adapt<IEnumerable<PollResponse>>();
            if (result is not null)
                return Result.Success(result);
            return Result.Failure<IEnumerable<PollResponse>>(PollError.PollNotFound);

        }
        public async Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken)
        {
           var poll = await _context.Polls.FindAsync(id , cancellationToken);
            
            if (poll is not null)
                return Result.Success(poll.Adapt<PollResponse>());
            return Result.Failure<PollResponse>(PollError.PollNotFound);



        }
        public async Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellationToken) {
            var poll = request.Adapt<Poll>();
           await _context.Polls.AddAsync(poll, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            var response = poll.Adapt<PollResponse>();
            return Result.Success<PollResponse>(response);
        }

        public async Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken)
        {
            var currentPOll = await _context.Polls.FindAsync(id,cancellationToken);
            if (currentPOll is null)
                return Result.Failure(PollError.PollNotFound);
            currentPOll.Title = poll.Title;
            currentPOll.Summary = poll.Summary;
            currentPOll.EndsAt = poll.EndsAt;
            currentPOll.StartsAt = poll.StartsAt;
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var currentPoll = await _context.Polls.FindAsync(id, cancellationToken);
            if (currentPoll is null)
                return Result.Failure(PollError.PollNotFound);
            _context.Polls.Remove(currentPoll);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> TogglePublishAsync(int id, CancellationToken cancellationToken)
        {
            var currentPOll = await _context.Polls.FindAsync(id, cancellationToken);
            if (currentPOll is null)
                return Result.Failure(PollError.PollNotFound);

            currentPOll.IsPublished = !currentPOll.IsPublished;
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }


}
