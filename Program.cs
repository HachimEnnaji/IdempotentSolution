using IdempotentApi.Abstractions;
using IdempotentApi.Abstractions.Attributes;
using IdempotentApi.Abstractions.Interfaces;
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
builder.Services.AddSingleton<IIdempotencyServiceCache, IdempotencyServiceCache>();
builder.Services.AddSingleton<IdempotencyAttribute>();

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

