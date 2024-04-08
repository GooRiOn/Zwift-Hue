using System.Text.Json.Serialization;

namespace ZwiftHue.Zwift;

public class ZwiftAuthData
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
}