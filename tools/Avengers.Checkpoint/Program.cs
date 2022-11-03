using Dapr.Client;
using System.Threading.Tasks;
var dapr = new DaprClientBuilder().Build();
string DAPR_STORE_NAME = "statestore";

int currentCount = 0;
while(true) {
    // var missions = await dapr.GetStateAsync<Mission[]>(DAPR_STORE_NAME, "missions");
    // foreach(var mission in missions) {
    //     System.Console.WriteLine($"Mission: {mission.Id} - {mission.Status} - {mission.PaymentStatus} - {mission.Currency} - {mission.Description}");
    // }
    currentCount++;
    System.Console.WriteLine ($"Mission {currentCount}");
    await dapr.PublishEventAsync("pubsub", "missions", new Mission() {
        Id = new Guid(),
        Status = "Completed",
        PaymentStatus = "Paid",
        Currency = "USD",
        Description = $"Mission {currentCount}"
    });
    await Task.Delay(2);

}
public class Mission
{
    public Guid Id => Guid.NewGuid();
    public string? Status { get; set; }
    public string? PaymentStatus { get; set; }
    public string? Currency { get; set; }
    public string? Description {get; set; }
}