namespace IdempotentApi.Abstractions.Interfaces;

public interface IIdempotencyServiceCache
{
    public Task<string?> GetKeyAsync(string key);
    public Task SetKeyAsync(string key, string value, TimeSpan? expiration = null);
}
