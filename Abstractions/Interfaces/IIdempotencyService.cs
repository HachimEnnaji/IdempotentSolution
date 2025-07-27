namespace IdempotentApi.Abstractions.Interfaces;

public interface IIdempotencyService
{
    public Task<string?> GetKeyAsync(string key);
    public Task SetKeyAsync(string key, string value, TimeSpan? expiration = null);
}
