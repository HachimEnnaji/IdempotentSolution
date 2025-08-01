using IdempotentApi.Abstractions.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace IdempotentApi.Abstractions;

public class RedisCacheService(IDistributedCache redis) : IIdempotencyServiceCache
{
    private readonly IDistributedCache _redis = redis
        ?? throw new ArgumentNullException($"{nameof(redis)} service cannot be null ");

    public async Task<string?> GetKeyAsync(string key)
    {
        var response = await _redis.GetStringAsync(key);

        return string.IsNullOrWhiteSpace(response) ? null : response.ToString();
    }

    public async Task SetKeyAsync(string key, string value, TimeSpan? expiration = null)
    {
        DistributedCacheEntryOptions expirationTime = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTimeOffset.UtcNow.Add(expiration
            ?? TimeSpan.FromMinutes(5))
        };

        await _redis.SetStringAsync(key, value, expirationTime, CancellationToken.None);
    }


}
