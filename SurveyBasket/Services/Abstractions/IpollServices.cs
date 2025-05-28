
using System.Threading;

namespace SurveyBasket.Services.Abstractions
{
    public interface IPollServices
    {
        public Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken);
        public Task<IEnumerable<PollResponse>> GetCurrentAsync(CancellationToken cancellationToken);

        public Task<Result<PollResponse>> GetAsync(int id,CancellationToken cancellationToken);

        public Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellationToken);

        public Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken);

        public Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);

        public Task<Result> TogglePublishAsync(int id, CancellationToken cancellationToken);

    }
}
