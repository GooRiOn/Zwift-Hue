using System.Text.Json.Serialization;

namespace ZwiftHue.Core.Infrastructure.Hue;

public class HueRequest
{
    [JsonPropertyName("on")]
    public bool On { get; set; } = true;
    
    [JsonPropertyName("hue")]
    public int Hue { get; set; }
    
    [JsonPropertyName("xy")]
    public float[] Xy { get; set; }

    [JsonPropertyName("effect")]
    public string Effect { get; set; } = HueEffects.None;
}