using System.Net.Http.Json;
using Grpc.Net.Client;
using ZwiftHue;
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
        int counter = 1;

        while (stoppingToken.IsCancellationRequested is false)
        {
            await Task.Delay(5_000, stoppingToken);
            
            var (isScrapped, data) = await _zwiftClient.GetActivityDataAsync(zwiftId, stoppingToken);

            if (isScrapped is false)
            {
                Console.WriteLine($"SKIP: {counter}");
                continue;
            }

            var powerZoneColor = ZwiftPowerZoneConverter.GetPowerZoneColor(200, data.Power);

            if (powerZoneColor.Zone == currentZone)
            {
                continue;
            }

            currentZone = powerZoneColor.Zone;
            await _hueClient.SetLightsColorAsync(powerZoneColor.Hue, powerZoneColor.Xy, stoppingToken);

            counter++;
        }
    }
}