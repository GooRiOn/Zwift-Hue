namespace ZwiftHue.Core.Infrastructure.Hue;

public class HueOptions
{
    public string UserId { get; set; }
    public string BridgeLocalIp { get; set; }
    public int[] LampIds { get; set; }
}