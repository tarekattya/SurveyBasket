using Microsoft.EntityFrameworkCore;
using SurveyBasket.Presistence.DbContextt;
using SurveyBasket.Services.NewFolder;

namespace SurveyBasket.Services
{
    public class PollServices(ApplicationDbContext context) : IPollServices
    {
        private readonly ApplicationDbContext _context = context;

        

        public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken) => await _context.Polls.AsNoTracking().ToListAsync();
        public async Task<Poll?> GetAsync(int id,CancellationToken cancellationToken) => await _context.Polls.FindAsync(id);
        public async Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken) {
            await _context.Polls.AddAsync(poll,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return poll;
        }

        public async Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellationToken)
        {
            var currentPOll = await GetAsync(id,cancellationToken);
            if (currentPOll is null)
                return false;
            currentPOll.Title = poll.Title;
            currentPOll.Summary = poll.Summary;
            currentPOll.EndsAt = poll.EndsAt;
            currentPOll.StartsAt = poll.StartsAt;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var currentPoll = await GetAsync(id, cancellationToken);
            if (currentPoll is null)
                return false;
            _context.Polls.Remove(currentPoll);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> TogglePublishAsync(int id,CancellationToken cancellationToken)
        {
            var currentPOll = await GetAsync(id, cancellationToken);
            if (currentPOll is null)
                return false;

            currentPOll.IsPublished = !currentPOll.IsPublished;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }


}
