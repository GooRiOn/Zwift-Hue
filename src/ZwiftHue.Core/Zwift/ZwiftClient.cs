using System.Text.Json;
using Microsoft.Extensions.Options;
using ZwiftHue.Core.Zwift.DTO;

namespace ZwiftHue.Core.Zwift;

public sealed class ZwiftClient
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<ZwiftOptions> _options;

    private ZwiftAuthData _authData;

    public ZwiftClient(HttpClient httpClient, IOptions<ZwiftOptions> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public async Task<bool> AuthenticateAsync(string username, string password, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _options.Value.Auth);
        request.Content = new FormUrlEncodedContent([
            new("username", username),
            new("password", password),
            new("grant_type", "password"),
            new("client_id", "Zwift_Mobile_Link")
        ]);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode is false)
        {
            return false;
        }
            
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        _authData = JsonSerializer.Deserialize<ZwiftAuthData>(json)!;
        return true;
    }

    public async Task<ZwiftProfileDto> GetProfileAsync(CancellationToken cancellationToken)
    {
        var url = $"{_options.Value.Host}/api/profiles/me";
        var request = CreateAuthorizedRequest(HttpMethod.Get, url);
        
        var response = await _httpClient.SendAsync(request, cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var profile = JsonSerializer.Deserialize<ZwiftProfileDto>(json, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });
        return profile;
    }

    public async Task<(bool isScreapped, ZwiftActivityData? data)> GetActivityDataAsync(int zwiftId, CancellationToken cancellationToken)
    {
        var url = $"{_options.Value.Host}/relay/worlds/1/players/{zwiftId}";
        var request = CreateAuthorizedRequest(HttpMethod.Get, url, "application/x-protobuf-lite");
        
        var response = await _httpClient.SendAsync(request, cancellationToken);
        
        if (response.IsSuccessStatusCode is false)
        {
            return (false, default);
        }
        
        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var playerState = PlayerState.Parser.ParseFrom(stream);
        Console.WriteLine($"Current power from stream : {playerState.Power}");

        return (true, new ZwiftActivityData(playerState.Power, (playerState.CadenceUHz * 60)/ 1_000_000, playerState.Heartrate));
    }

    private HttpRequestMessage CreateAuthorizedRequest(HttpMethod method, string url, string accept = "application/json")
    {
        var request = new HttpRequestMessage(method, url);
        request.Headers.Add("Authorization", $"Bearer {_authData.AccessToken}");
        request.Headers.Add("Accept", accept);
        request.Headers.Add("User-Agent", "Zwift/115 CFNetwork/758.0.2 Darwin/15.0.0");
        
        return request;
    }
}