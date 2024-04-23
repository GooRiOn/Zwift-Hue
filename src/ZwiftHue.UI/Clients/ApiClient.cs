using System.Net.Http.Json;
using ZwiftHue.UI.Pages;

namespace ZwiftHue.UI.Clients;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    
    //TODO:Extract to appsettings.json
    private const string Host = "http://localhost:5210";

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(Host);
    }

    public async Task<bool> LoginAsync(Home.RiderLoginModel model)
    {
        var response = await _httpClient.PostAsync("/login", JsonContent.Create(model));
        return response.IsSuccessStatusCode;
    }
}