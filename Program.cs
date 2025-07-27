using IdempotentApi.Abstractions;
using IdempotentApi.Abstractions.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); // Necessario per Minimal API
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IIdempotencyService, IdempotencyService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/", () =>
{
})
.WithOpenApi();

app.Run();

