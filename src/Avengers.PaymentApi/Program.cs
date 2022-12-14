using Bogus;
using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddApplicationMonitoring();

var dapr = new DaprClientBuilder().Build();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCloudEvents();

var paymentFaker = new Faker<Payment>()
    .StrictMode(true)
    .RuleFor(m => m.Amount, (f, m) => f.Finance.Amount(1000000, 10000000));

var payment = paymentFaker.Generate().Amount;

app.MapGet("/payment/{missionId}",  (Guid missionId) =>
{
    return Results.Accepted("/payment", payment);

}).WithTopic("pubsub", "payment");

app.MapPost("/payment/{missionId}",  (Guid missionId) =>
{
    return Results.Accepted("/payment", payment);
}).WithTopic("pubsub", "payment");

app.Run();

public class Payment
{
    public Decimal Amount { get; set; }
}