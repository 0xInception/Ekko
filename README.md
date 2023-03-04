# Ekko
Ekko is a small library that allows you to interact with the LCU API (The API that the League Client exposes). It is compatible with Windows, Linux, and MacOS and it targets .NET 7 and .NET Standard 2.0.

## Example
### Here is a simple icon changer.
```csharp
using System.Text;
using Ekko;

Console.Title = "SimpleIconChanger";
Console.ForegroundColor = ConsoleColor.White;
var token = new CancellationTokenSource();
var watcher = new LeagueClientWatcher();
watcher.OnLeagueClient += async (clientWatcher, client) =>
{
    Console.Clear();
    token.Cancel();
    var api = new LeagueApi(client);
    Console.WriteLine("Making sure you are logged in...");
    await api.EnsureLoggedIn();
    Console.Clear();
    Console.WriteLine("Input the console icon you want (default 29):");
    var input = Console.ReadLine();
    int icon = 29;
    if (Int32.TryParse(input, out var output))
    {
        icon = output;
    }

    var response = await api.SendAsync(HttpMethod.Put, "/lol-summoner/v1/current-summoner/icon", new StringContent($"{{\"profileIconId\":{icon}}}",Encoding.UTF8,"application/json"));
    if (string.IsNullOrEmpty(response) || !response.Contains($"profileIconId\":{icon}")) 
        Console.WriteLine("Fail!");
    else
        Console.WriteLine("Success!");
    Console.ReadLine();
    Environment.Exit(0);
};
Console.WriteLine("Waiting for league client!");
await watcher.Observe(token.Token);
await Task.Delay(-1);
```
