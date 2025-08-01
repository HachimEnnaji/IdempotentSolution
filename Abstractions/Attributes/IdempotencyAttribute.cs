using IdempotentApi.Abstractions.Interfaces;
using IdempotentApi.Domain;
using IdempotentApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace IdempotentApi.Abstractions.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class IdempotencyAttribute(IIdempotencyServiceCache cache, int ttlMinutes = 60) : Attribute, IAsyncActionFilter
{
    private readonly int _ttl = ttlMinutes;
    private readonly IIdempotencyServiceCache _cache = cache
        ?? throw new Exception("Missing IIdempotency Service");
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();
        var key = context.HttpContext.Request.Headers["Idempotency-Key"].ToString();

        if (string.IsNullOrWhiteSpace(key))
        {
            context.Result = new BadRequestObjectResult("Idempotency-Key header is required.");
            return;
        }

        var cached = await _cache.GetKeyAsync(key);

        if (cached is not null)
        {
            var response = Json.SafeDeserialize<IdempotencyResponse>(cached);
            context.Result = new ObjectResult(response)
            {
                StatusCode = response?.IdemPotencyStatusCode ?? 200,
            };

            Console.WriteLine($"\nIdempotency Key: {key} processed in {stopwatch.ElapsedMilliseconds} ms and the value was cached");

            return;
        }

        var execution = await next();

        if (execution.Result is ObjectResult objectResult)
        {

            var response = new IdempotencyResponse
            {
                Result = objectResult.Value,
                IdemPotencyStatusCode = objectResult.StatusCode ?? 200,
            };

            var serialized = Json.SafeSerialize(response);

            await _cache.SetKeyAsync(key, serialized, TimeSpan.FromSeconds(_ttl));
        }

        stopwatch.Stop();
        Console.WriteLine($"\nIdempotency Key: {key} processed in {stopwatch.ElapsedMilliseconds} ms and the value wasn't cached");
    }
}
