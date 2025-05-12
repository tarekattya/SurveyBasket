
using System.Threading;

namespace SurveyBasket.Services.NewFolder
{
    public interface IPollServices
    {
        public Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken);

        public Task<Poll?> GetAsync(int id,CancellationToken cancellationToken);

        public Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken);

        public Task<bool> UpdateAsync(int id, Poll poll , CancellationToken cancellationToken);

        public Task<bool> DeleteAsync(int id , CancellationToken cancellationToken);

        public Task<bool> TogglePublishAsync(int id, CancellationToken cancellationToken);

    }
}
