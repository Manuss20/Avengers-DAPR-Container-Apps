using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IAvengerBackendClient, AvengerBackendClient>();
builder.Services.AddMemoryCache();
builder.Services.AddApplicationMonitoring();

var baseURL = (Environment.GetEnvironmentVariable("BASE_URL") ?? "http://localhost") + ":" + (Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3500");
builder.Services.AddHttpClient("Missions", (httpClient) =>
{
    httpClient.BaseAddress = new Uri(baseURL);
    httpClient.DefaultRequestHeaders.Add("dapr-app-id", "Missions");
});

builder.Services.AddHttpClient("Payment", (httpClient) =>
{
    httpClient.BaseAddress = new Uri(baseURL);
    httpClient.DefaultRequestHeaders.Add("dapr-app-id", "Payment");
});

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
    public string MissionId { get; set; }
    public string Status { get; set; }
    public Decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Description {get; set; }
}

public interface IAvengerBackendClient
{
    [Get("/missions")]
    Task<List<Mission>> GetMissions();

    [Get("/payment/{missionId}")]
    Task<Decimal> GetPayment(string missionId);
}

public class AvengerBackendClient : IAvengerBackendClient
{
    IHttpClientFactory _httpClientFactory;

    public AvengerBackendClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Decimal> GetPayment(string missionId)
    {
        var client = _httpClientFactory.CreateClient("Payment");
        return await RestService.For<IAvengerBackendClient>(client).GetPayment(missionId);
    }

    public async Task<List<Mission>> GetMissions()
    {
        var client = _httpClientFactory.CreateClient("Missions");
        return await RestService.For<IAvengerBackendClient>(client).GetMissions();
    }
}