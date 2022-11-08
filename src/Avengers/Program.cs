using Dapr.Client;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IAvengerBackendClient, AvengerBackendClient>();
builder.Services.AddMemoryCache();
builder.Services.AddDaprClient();
builder.Services.AddApplicationMonitoring();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

public class Mission
{
    public Guid Id { get; set; }
    public string? Status { get; set; }
    public string? PaymentStatus { get; set; }
    public Decimal Amount { get; set; }
    public string? Currency { get; set; }
    public string? Description { get; set; }
}

public interface IAvengerBackendClient
{
    [Get("/missions")]
    Task<List<Mission>> GetMissions();

    [Get("/payment/{missionId}")]
    Task<Decimal> GetPayment(Guid missionId);
}

public class AvengerBackendClient : IAvengerBackendClient
{
    DaprClient _daprClient;

    public AvengerBackendClient(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task<Decimal> GetPayment(Guid missionId)
    {
        return await _daprClient.InvokeMethodAsync<Guid, Decimal>(HttpMethod.Get, "Payment", "payment/" + missionId.ToString());
    }

    public async Task<List<Mission>> GetMissions()
    {
        return await _daprClient.InvokeMethodAsync<List<Mission>>(HttpMethod.Get, "Missions", "missions");
    }
}
