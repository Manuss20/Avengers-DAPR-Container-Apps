using Bogus;

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

var statusprogess = new[] { "Completed", "In progress", "Canceled"};

var missionsFaker = new Faker<Mission>()
    .StrictMode(true)
    .RuleFor(m => m.Id, (f, m) => f.Database.Random.Guid())
    .RuleFor(m => m.Status, (f, m) => f.PickRandom(statusprogess))
    .RuleFor(m => m.PaymentStatus, (f, m) => f.Finance.TransactionType())
    .RuleFor(m => m.Currency, (f, m) => f.Finance.Currency().Symbol)
    .RuleFor(m => m.Description, (f, m) => f.Lorem.Paragraphs(1));

var missions = missionsFaker.Generate(10);

app.MapGet("/missions", () => Results.Ok(missions))
    .Produces<Mission[]>(StatusCodes.Status200OK)
    .WithName("GetMissions");

app.Run();

public class Mission
{
    public Guid Id => Guid.NewGuid();
    public string? Status { get; set; }
    public string? PaymentStatus { get; set; }
    public string? Currency { get; set; }
    public string? Description {get; set; }
    
}