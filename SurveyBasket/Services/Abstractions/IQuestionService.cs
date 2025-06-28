using SurveyBasket.Contracts.Common;

namespace SurveyBasket.Services.Abstractions
{
    public interface IQuestionService
    {

        Task<Result<PageinatedList<QuestionResponse>>> GetAllAsync(int Pollid, FilterRequest request, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<QuestionResponse>>> GetAvaliableAll(int Pollid,string UserId, CancellationToken cancellationToken = default);

        Task<Result<QuestionResponse>> GetAsync(int Pollid, int id, CancellationToken cancellationToken = default);

        public Task<Result<QuestionResponse>> AddAsync( int Pollid , QuestionRequest request, CancellationToken cancellationToken = default); 
        public Task<Result> UpdateAsync( int Pollid , int id , QuestionRequest request, CancellationToken cancellationToken = default); 



        public Task<Result> ToggleStatus(int Pollid, int id, CancellationToken cancellationToken = default);
    }
}
