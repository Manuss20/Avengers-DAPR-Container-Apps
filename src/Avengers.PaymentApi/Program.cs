using Bogus;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationMonitoring();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var paymentFaker = new Faker<Payment>()
    .StrictMode(true)
    .RuleFor(m => m.Amount, (f, m) => f.Finance.Amount(1000000, 10000000));

app.MapGet("/payment/{missionId}", (Guid missionId) =>
{
    decimal paymentValue = 0;   
    paymentValue = paymentFaker.Generate().Amount;
    return Results.Ok(paymentValue);
})
.Produces<int>(StatusCodes.Status200OK)
.WithName("GetPayment");

app.Run();

public class Payment
{
    public Decimal Amount { get; set; }
}