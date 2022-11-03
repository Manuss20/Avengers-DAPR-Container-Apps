using Bogus;
using Dapr.Client;

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

app.UseCloudEvents();

var dapr = new DaprClientBuilder().Build();
var statusprogess = new[] { "Completed", "In progress", "Canceled"};

var missionsFaker = new Faker<Mission>()
    .StrictMode(true)
    .RuleFor(m => m.Id, (f, m) => f.Database.Random.Guid())
    .RuleFor(m => m.Status, (f, m) => f.PickRandom(statusprogess))
    .RuleFor(m => m.PaymentStatus, (f, m) => f.Finance.TransactionType())
    .RuleFor(m => m.Currency, (f, m) => f.Finance.Currency().Symbol)
    .RuleFor(m => m.Description, (f, m) => f.Lorem.Paragraphs(1));

var genmissions = missionsFaker.Generate(10);
string DAPR_STORE_NAME = "statestore";

app.MapGet("/missions", async () =>
{
    await dapr.GetStateAsync<Mission[]>(DAPR_STORE_NAME, "missions");
    return Results.Accepted("/missions", genmissions);
} )
    .Produces<Mission[]>(StatusCodes.Status200OK)
    .WithName("GetMissions");

app.MapPost("/missions" , async (Mission mission) => 
{
    await dapr.SaveStateAsync(DAPR_STORE_NAME, "missions", genmissions);
    return Results.Accepted("/missions", genmissions);
}).WithTopic("pubsub", "missions");

app.Run();

public class Mission
{
    public Guid Id => Guid.NewGuid();
    public string? Status { get; set; }
    public string? PaymentStatus { get; set; }
    public string? Currency { get; set; }
    public string? Description {get; set; }
    
}