namespace ZwiftHue.Core.Zwift;

//https://zwiftinsider.com/power-zone-colors/

public static class ZwiftPowerZoneConverter
{
    public static ZwiftPowerZoneColor GetPowerZoneColor(int ftp, int power)
    {
        var ratio = Math.Ceiling(power * 100f/ ftp);

        return ratio switch
        {
            < 60 => new("grey",4000, [0.3144f, 0.3225f]),
            < 75 => new("blue", 21195, [0.1544f, 0.0931f]),
            < 89 => new("green", 47104, [0.2597f, 0.6326f]),
            < 104 => new("yellow", 11151, [0.462f, 0.4761f]),
            < 118 => new("orange", 5972, [0.5698f, 0.3997f]),
            > 118 => new("red", 240, [0.6866f, 0.312f]),
            _ => new("grey",4000, [0.3144f, 0.3225f])
        };
    }
}