using System.Net.Http.Json;
using System.Text.Json;
using ZwiftHue.UI.Models;
using ZwiftHue.UI.Models.Write;

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

    public async Task<bool> LoginAsync(RiderLoginModel model)
    {
        var response = await _httpClient.PostAsync("/login", JsonContent.Create(model));
        return response.IsSuccessStatusCode;
    }
    
    public async Task<ZwiftProfileDto> GetProfileAsync()
    {
        var response = await _httpClient.GetAsync("/me");

        if (response.IsSuccessStatusCode is false)
        {
            return default;
        }
        
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ZwiftProfileDto>(json, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        })!;
    }
    
    public async Task StartActivityAsync(int userId)
    {
        var response = await _httpClient.PostAsync($"/activity/{userId}/start", default);
    }
}