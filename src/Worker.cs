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
        
        await _hueClient.SetLightsColorAsync(123, [0, 0], stoppingToken);

        
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
        


        await foreach (var data in _zwiftClient.GetActivityDataAsync(zwiftId, stoppingToken))
        {
            var powerZoneColor = ZwiftPowerZoneConverter.GetPowerZoneColor(60, data.Power);
            await _hueClient.SetLightsColorAsync(powerZoneColor.hue, powerZoneColor.xy, stoppingToken);
        }
    }
}