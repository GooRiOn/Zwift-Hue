using ZwiftHue.Hue;
using ZwiftHue.Zwift;

namespace ZwiftHue;

public class Worker : BackgroundService
{
    private readonly ZwiftClient _zwiftClient;
    private readonly HueClient _hueClient;

    public Worker(ZwiftClient zwiftClient,  HueClient hueClient)
    {
        _zwiftClient = zwiftClient;
        _hueClient = hueClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _hueClient.InitAsync(stoppingToken);
        
        string username, password, zwiftId;
        
        while (true)
        {
            Console.WriteLine("Zwift username");
            username = Console.ReadLine();
        
            Console.WriteLine("Zwift password");
            password = Console.ReadLine();
            
            Console.WriteLine("Zwift ID");
            zwiftId = Console.ReadLine();
        
            var isSucceeded = await _zwiftClient.AuthenticateAsync(username, password, stoppingToken);
        
            if (isSucceeded)
            {
                break;
            }
            
            Console.WriteLine("LOGIN FAILED. Press any key to retry...");
            Console.ReadKey();
            Console.Clear();
        }

        string currentZone = "";
        int offTheZone = 0;
        int delay = 1_000;

        while (stoppingToken.IsCancellationRequested is false)
        {
            await Task.Delay(delay, stoppingToken);
            
            var (isScrapped, data) = await _zwiftClient.GetActivityDataAsync(zwiftId, stoppingToken);

            if (isScrapped is false)
            {
                continue;
            }

            var powerZoneColor = ZwiftPowerZoneConverter.GetPowerZoneColor(135, data.Power);

            if (powerZoneColor.Zone == currentZone)
            {
                offTheZone = 0;
                delay = 1_000;
                continue;
            }
            
            offTheZone++;

            if (offTheZone < 2)
            {
                delay = 1_000;
                continue;
            }

            currentZone = powerZoneColor.Zone;
            delay = 5_000;
            await _hueClient.SetLightsColorAsync(powerZoneColor.Hue, powerZoneColor.Xy, HueEffects.None, stoppingToken);
        }
    }
}