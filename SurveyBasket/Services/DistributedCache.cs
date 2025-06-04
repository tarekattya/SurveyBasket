using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace SurveyBasket.Services
{
    public class CacheService(IDistributedCache distributedCache) : ICacheService
    {
        private readonly IDistributedCache _distributedCache = distributedCache;

        public async Task<T?> GetAsync<T>(string Key, CancellationToken cancellationToken) where T : class
        {
            var cachedValue = await _distributedCache.GetStringAsync(Key,cancellationToken);

            return string.IsNullOrEmpty(cachedValue) ? null : JsonSerializer.Deserialize<T>(cachedValue);
            
        }


        public async Task SetAsync<T>(string Key, T value, CancellationToken cancellationToken) where T : class
        {
             await _distributedCache.SetStringAsync(Key, JsonSerializer.Serialize(value), cancellationToken);
        }
        public async Task RemoveAsync(string Key, CancellationToken cancellationToken)
        {

            await _distributedCache.RemoveAsync(Key, cancellationToken);

        }
    }
}
