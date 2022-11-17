using System.Net.Http.Headers;
using System.Text;

namespace Ekko;

public class LeagueApi
{
    private readonly string _authToken;
    private readonly int _authPort;
    private HttpClient _client;
    private readonly string _baseUrl;

    public LeagueApi(string authToken,int authPort)
    {
        _authToken = authToken;
        _authPort = authPort;
        _client = new HttpClient(new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        });
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{authToken}")));
        _client.DefaultRequestHeaders.Add("User-Agent", "LeagueOfLegendsClient");
        _client.DefaultRequestHeaders.Add("Accept","application/json");
        _baseUrl = $"https://127.0.0.1:{_authPort}";
    }
    public async Task<string?> SendAsync(HttpMethod method,string endpoint,HttpContent? body = null)
    {
        var req = new HttpRequestMessage(method, $"{_baseUrl}{endpoint}");
        if (body is not null)
            req.Content = body;
        var result = await _client.SendAsync(req);
        return await result.Content.ReadAsStringAsync();
    }
}