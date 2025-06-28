using Hangfire;
using SurveyBasket.Contracts.Common;
using SurveyBasket.Presistence.DbContextt;
using SurveyBasket.Presistence.Entites;

namespace SurveyBasket.Services
{
    public class PollServices(ApplicationDbContext context , INotifacitionServices notifacitionServices) : IPollServices
    {
        private readonly ApplicationDbContext _context = context;
        private readonly INotifacitionServices _notifacitionServices = notifacitionServices;

        public async Task<PageinatedList<PollResponse>> GetAllAsync(FilterRequest request, CancellationToken cancellationToken) 
          {


            var polls = _context.Polls
            .Where( p => String.IsNullOrEmpty(request.SearchValue) || p.Title.Contains(request.SearchValue))
            .AsNoTracking()
            .ProjectToType<PollResponse>();

            var response = await PageinatedList<PollResponse>.CreateAsync(polls, request.pageSize, request.pageNumber, cancellationToken);

            return response;
        }


        public async Task<PageinatedList<PollResponse>> GetCurrentAsync(FilterRequest request, CancellationToken cancellationToken)
        {
            var CurrentPolls = _context.Polls
                 .Where(p => p.IsPublished && p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow)
                 && (String.IsNullOrEmpty(request.SearchValue) || p.Title.Contains(request.SearchValue)))
                .AsNoTracking()
                .ProjectToType<PollResponse>();

            var response = await PageinatedList<PollResponse>.CreateAsync(CurrentPolls, request.pageSize, request.pageNumber, cancellationToken);
            return response;

        }

        public async Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken)
        {


           var poll = await _context.Polls.FindAsync(id , cancellationToken);

            if (poll is not null)
                return Result.Success(poll.Adapt<PollResponse>());
            return Result.Failure<PollResponse>(PollErrors.PollNotFound);



        }
        public async Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellationToken) {
            var poll = request.Adapt<Poll>();
            var IsExists = await _context.Polls.AnyAsync(x => x.Title == request.Title, cancellationToken);
            if (IsExists)
                return Result.Failure<PollResponse>(PollErrors.DublicateTitles);
            await _context.Polls.AddAsync(poll, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            var response = poll.Adapt<PollResponse>();
            return Result.Success<PollResponse>(response);
        }

        public async Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken)
        {
            var currentPOll = await _context.Polls.FindAsync(id,cancellationToken);
            if (currentPOll is null)
                return Result.Failure(PollErrors.PollNotFound);
            var IsExists = await _context.Polls.AnyAsync( x => x.Id != id && x.Title == poll.Title, cancellationToken);
            if (IsExists)
                return Result.Failure<PollResponse>(PollErrors.DublicateTitles);
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
                return Result.Failure(PollErrors.PollNotFound);
            _context.Polls.Remove(currentPoll);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> TogglePublishAsync(int id, CancellationToken cancellationToken)
        {
            var currentPOll = await _context.Polls.FindAsync(id, cancellationToken);
            if (currentPOll is null)
                return Result.Failure(PollErrors.PollNotFound);

            currentPOll.IsPublished = !currentPOll.IsPublished;
            await _context.SaveChangesAsync(cancellationToken);

            if(currentPOll.IsPublished && currentPOll.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
                    BackgroundJob.Enqueue(() => _notifacitionServices.SendNotificationAsync(currentPOll.Id));

            return Result.Success();
        }
    }


}
