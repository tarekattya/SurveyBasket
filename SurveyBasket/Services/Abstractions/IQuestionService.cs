namespace SurveyBasket.Services.Abstractions
{
    public interface IQuestionService
    {

        Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int Pollid, CancellationToken cancellationToken = default);

        Task<Result<QuestionResponse>> GetAsync(int Pollid, int id, CancellationToken cancellationToken = default);

        public Task<Result<QuestionResponse>> AddAsync( int Pollid , QuestionRequest request, CancellationToken cancellationToken = default); 
        public Task<Result> UpdateAsync( int Pollid , int id , QuestionRequest request, CancellationToken cancellationToken = default); 



        public Task<Result> ToggleStatus(int Pollid, int id, CancellationToken cancellationToken = default);
    }
}
