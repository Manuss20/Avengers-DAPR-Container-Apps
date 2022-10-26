using Bogus;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddApplicationMonitoring();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var paymentFaker = new Faker<Mission>()
    .StrictMode(true)
    .RuleFor(m => m.Amount, (f, m) => f.Finance.Amount(1-100000));

app.MapGet("/payment/{missionId}", (string missionId, IMemoryCache memoryCache) =>
{
    var memCacheKey = $"{missionId}-payment";
    int paymentValue = -404;
    
    if(!memoryCache.TryGetValue(memCacheKey, out inventoryValue))
    {
        //paymentValue = new Random().Next(1, 100000);
        paymentValue = paymentFaker;
        memoryCache.Set(memCacheKey, paymentValue);
    }

    paymentValue = memoryCache.Get<int>(memCacheKey);

    return Results.Ok(paymentValue);
})
.Produces<int>(StatusCodes.Status200OK)
.WithName("GetPayment");

app.Run();
