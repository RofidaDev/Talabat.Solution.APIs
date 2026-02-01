using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Services.Contract;

namespace Talabat.Services
{
    public class ResponseCacheService : IResponseCacheService
    {private readonly IDatabase _database;
        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response is null) return; //get out of method
            var serializeOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var serializedResponse = JsonSerializer.Serialize(response,serializeOptions);
            await _database.StringSetAsync(cacheKey,serializedResponse,timeToLive);   //cache response
        }

        public async Task<string?> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponce = await _database.StringGetAsync(cacheKey);
            if (cachedResponce.IsNullOrEmpty) return null;
            return cachedResponce;
        }
    }
}
