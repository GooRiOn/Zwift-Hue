namespace ZwiftHue.Core.Infrastructure.ProfileConfigs;

public class RiderProfileConfiguration
{
    public int Id { get; set; }
    public bool LightsOnActivityStart { get; set; } = true;
    public int PowerZoneMissRatio { get; set; } = 3;
    public int PowerRefreshMilliseconds { get; set; } = 1_000;
}