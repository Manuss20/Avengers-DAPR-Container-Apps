using Dapr.Client;

for (int i = 1; i <= 10; i++) {
    using var client = new DaprClientBuilder().Build();
    await client.InvokeMethodAsync<List<Mission>>(HttpMethod.Get, "Missions", "missions");
    await client.InvokeMethodAsync<string, Decimal>(HttpMethod.Get, "Payment", "payment/", Guid.NewGuid().ToString());

    Console.WriteLine("Published data: ");
    await Task.Delay(TimeSpan.FromSeconds(1));
}

public class Mission
{
    public Guid Id { get; set; }
    public string? Status { get; set; }
    public string? PaymentStatus { get; set; }
    public Decimal Amount { get; set; }
    public string? Currency { get; set; }
    public string? Description { get; set; }
}