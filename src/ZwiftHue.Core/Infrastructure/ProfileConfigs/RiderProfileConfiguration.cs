namespace ZwiftHue.Core.Infrastructure.ProfileConfigs;

public class RiderProfileConfiguration
{
    public int Id { get; set; }
    public bool LightsOnActivityStart { get; set; } = true;
    public int PowerZoneDiffPercentageToleration { get; set; }= 5;
}