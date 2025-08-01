using IdempotentApi.Abstractions;
using IdempotentApi.Abstractions.Attributes;
using IdempotentApi.Abstractions.Interfaces;
using IdempotentApi.Configuraations;
using IdempotentApi.Headers;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<AddIdempotencyHeaderoperationFilter>();
}
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection(nameof(RedisSettings)));

builder.Services.AddSingleton<IIdempotencyServiceCache, RedisCacheService>();
builder.Services.AddSingleton<IdempotencyAttribute>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    string connection = builder.Configuration.GetConnectionString("RedisConnection") ??
                        throw new ArgumentNullException("RedisConnection", "Connection string for Redis is not configured.");
    options.Configuration = connection;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

