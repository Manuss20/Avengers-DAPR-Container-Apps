// See https://aka.ms/new-console-template for more information

int clientNumber = 0;

Console.WriteLine("Massive client conections in C#");
Console.WriteLine("--------------------------------");
Console.WriteLine("Enter the number of clients to connect to the web");
clientNumber = Convert.ToInt32(Console.ReadLine());

var recurisveClient = new CustomClients();
recurisveClient.GetInParallel(clientNumber);

bool exit = true;

while (exit)
{

    Task.Delay(5 * 1000).ContinueWith(t => Console.WriteLine(" ----- Starting new clients ----"));
    Task.Delay(5 * 1000).ContinueWith(t => recurisveClient.GetInParallel(clientNumber));
}

public class CustomClients
{

    private HttpClient client;

    public CustomClients()
    {
        client = new HttpClient();               
    }
        
    public async Task<string> CallWeb(int ids)
    {
        Console.WriteLine("Calling client {0}", ids + 1);

        var url = "https://avengers.kindsea-ad48aa1f.westeurope.azurecontainerapps.io/missions";

        var response = await client
            .GetAsync(url)
            .ConfigureAwait(false);

        var res = await response.Content.ReadAsStringAsync();

       

        Console.WriteLine("Response client {0}", ids + 1 );

        return res;
    }

    public async void GetInParallel(int clientNumbers)
    {
        var tasks = new List<Task<string>>();

        for (int i = 0; i < clientNumbers; i++)
        {
            tasks.Add(CallWeb(i));
        }

        var results = await Task.WhenAll(tasks);
        
    }

}

















