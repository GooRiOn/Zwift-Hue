using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZwiftHue.Core.Commands.StartActivity;
using ZwiftHue.Core.Exceptions;
using ZwiftHue.Core.Infrastructure.Channels;
using ZwiftHue.Core.Infrastructure.Hue;

namespace ZwiftHue.Core.Infrastructure.Zwift;

public class ZwiftActivityWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public ZwiftActivityWorker(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scope = _serviceProvider.CreateScope();
        var channel = scope.ServiceProvider.GetRequiredService<IMessageChannel>();

        var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);

        await foreach (var message in channel.GetAsync(stoppingToken))
        {
            var command = JsonSerializer.Deserialize(message.SerializedMessage, message.Type);

            _ = command switch
            {
                StartActivity c => ProcessActivityAsync(c.UserId, cancellationTokenSource.Token),
                _ => throw new ZwiftHueException($"Channel message of type {message.Type.Name} is not supported!")
            };
        }
    }
    

    private async Task ProcessActivityAsync(int userId, CancellationToken stoppingToken)
    {
        var scope = _serviceProvider.CreateScope();
        var hueClient = scope.ServiceProvider.GetRequiredService<HueClient>();
        var zwiftClient = scope.ServiceProvider.GetRequiredService<ZwiftClient>();
        
        await hueClient.InitAsync(stoppingToken);
        
        string currentZone = "";
        int offTheZone = 0;
        int delay = 1_000;

        while (stoppingToken.IsCancellationRequested is false)
        {
            await Task.Delay(delay, stoppingToken);
            
            var (isScrapped, data) = await zwiftClient.GetActivityDataAsync(userId, stoppingToken);

            if (isScrapped is false)
            {
                continue;
            }

            var powerZoneColor = ZwiftPowerZoneConverter.GetPowerZoneColor(238, data.Power);

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
            await hueClient.SetLightsColorAsync(powerZoneColor.Hue, powerZoneColor.Xy, HueEffects.None, stoppingToken);
        }
    }
}