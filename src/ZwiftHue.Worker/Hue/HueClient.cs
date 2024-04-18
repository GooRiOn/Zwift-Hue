using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace ZwiftHue.Hue;

public sealed class HueClient
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<HueOptions> _options;

    public HueClient(HttpClient httpClient, IOptions<HueOptions> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public Task InitAsync(CancellationToken cancellationToken)
        => SetLightsColorAsync(0, [0f,0f], HueEffects.Colorloop, cancellationToken);


    public async Task SetLightsColorAsync(int hue, float[] xy, string effect, CancellationToken cancellationToken)
    {
        var hueRequest = new HueRequest
        {
            Hue = hue,
            Xy = xy,
            Effect = effect
        };
        
        var json = JsonSerializer.Serialize(hueRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var requests = _options.Value.LampIds.Select(x =>
        {
            var message = new HttpRequestMessage(HttpMethod.Put, $"http://{_options.Value.BridgeLocalIp}/api/{_options.Value.UserId}/lights/{x}/state");
            message.Content = content;
            return message;
        });
        
        var tasks = requests.Select(x => _httpClient.SendAsync(x, cancellationToken));
        await Task.WhenAll(tasks);
    }
}