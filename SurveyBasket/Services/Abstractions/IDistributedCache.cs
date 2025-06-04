namespace SurveyBasket.Services.Abstractions
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string Key, CancellationToken cancellationToken) where T : class;
        Task SetAsync<T>(string Key, T value, CancellationToken cancellationToken) where T : class;
        Task RemoveAsync(string Key, CancellationToken cancellationToken);


    }
}
