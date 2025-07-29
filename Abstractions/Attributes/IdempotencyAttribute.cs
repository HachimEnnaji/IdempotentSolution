using IdempotentApi.Abstractions.Interfaces;
using IdempotentApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace IdempotentApi.Abstractions.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class IdempotencyAttribute(IIdempotencyServiceCache cache, int ttlMinutes = 60) : Attribute, IAsyncActionFilter
{
    private readonly int _ttl = ttlMinutes;
    private readonly IIdempotencyServiceCache _cache = cache
        ?? throw new Exception("Missing IIdempotency Service");
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var key = context.HttpContext.Request.Headers["Idempotency-Key"].ToString();

        if (string.IsNullOrWhiteSpace(key))
        {
            context.Result = new BadRequestObjectResult("Idempotency-Key header is required.");
            return;
        }

        //var cache = context.HttpContext.RequestServices.GetRequiredService<IIdempotencyService>();

        var cached = await _cache.GetKeyAsync(key);

        if (cached is not null)
        {
            var response = JsonSerializer.Deserialize<IdempotencyResponse>(cached);
            context.Result = new ObjectResult(response)
            {
                StatusCode = response?.IdemPotencyStatusCode ?? 200,
            };
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

            var serialized = JsonSerializer.Serialize(response);

            await _cache.SetKeyAsync(key, serialized, TimeSpan.FromSeconds(_ttl));
        }

    }
}
