using IdempotentApi.Abstractions.Interfaces;
using StackExchange.Redis;

namespace IdempotentApi.Abstractions;

public class RedisCacheService(IConnectionMultiplexer redis) : IIdempotencyServiceCache
{
    private readonly IDatabase _redis = redis.GetDatabase()
        ?? throw new ArgumentNullException(nameof(redis));

    public async Task<bool> HasResponseAsync(string key)
       => await _redis.KeyExistsAsync(key);
    public async Task<string?> GetKeyAsync(string key)
    {

        var response = await _redis.StringGetAsync(key);

        return response.HasValue ? response.ToString() : null;
    }

    public async Task SetKeyAsync(string key, string value, TimeSpan? expiration = null)
    {
        await _redis.StringSetAsync(key, value, expiration ?? TimeSpan.FromMinutes(5));
    }


}
