using ZwiftHue.Core.Infrastructure.ProfileConfigs;

namespace ZwiftHue.Core.Infrastructure.Zwift;

public interface IZwiftPowerZoneConverter
{
    ZwiftPowerZoneColor? ConvertPowerZoneColor(int power, int ftp, RiderProfileConfiguration configuration);
}