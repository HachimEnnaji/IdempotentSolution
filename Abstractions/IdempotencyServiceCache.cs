using IdempotentApi.Abstractions.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace IdempotentApi.Abstractions;

public class IdempotencyServiceCache(IMemoryCache cache) : IIdempotencyServiceCache
{
    private readonly IMemoryCache _cache = cache
        ?? throw new ArgumentNullException(nameof(cache));

    public Task<string?> GetKeyAsync(string key)
    {
        _cache.TryGetValue(key, out string? value);
        return Task.FromResult(value);
    }

    public Task SetKeyAsync(string key, string value, TimeSpan? expiration = null)
    {
        _cache.Set(key, value, expiration
            ?? TimeSpan.FromMinutes(5));
        return Task.CompletedTask;
    }
}
