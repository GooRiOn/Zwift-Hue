using System.Text.Json;
using Microsoft.Extensions.Options;
using ZwiftHue.Exceptions;

namespace ZwiftHue.Zwift;

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

    public async IAsyncEnumerable<ZwiftActivityData> GetActivityDataAsync(int zwiftId, CancellationToken cancellationToken)
    {
        var url = $"{_options.Value.Host}/relay/worlds/1/players/{zwiftId}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Authorization", $"Bearer {_authData.AccessToken}");
        request.Headers.Add("Accept", $"application/x-protobuf-lite");
        request.Headers.Add("User-Agent", "Zwift/115 CFNetwork/758.0.2 Darwin/15.0.0");

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode is false)
        {
            throw new ZwiftHueException("Could not scrap activity data");
        }

        while (cancellationToken.IsCancellationRequested is false)
        {
            var stream = await request.Content.ReadAsStreamAsync(cancellationToken);
            var playerState = PlayerState.Parser.ParseFrom(stream);
            yield return new ZwiftActivityData(playerState.Power, (playerState.CadenceUHz * 60)/ 1_000_000, playerState.Heartrate);
            await Task.Delay(3_000, cancellationToken);
        }
    }
}