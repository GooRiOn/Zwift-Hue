using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZwiftHue.Core.Commands.StartActivity;
using ZwiftHue.Core.Exceptions;
using ZwiftHue.Core.Infrastructure.Channels;
using ZwiftHue.Core.Infrastructure.Hue;
using ZwiftHue.Core.Infrastructure.ProfileConfigs;

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
        var configProvider = scope.ServiceProvider.GetRequiredService<IRiderProfileConfigurationProvider>();
        var powerZoneConverter= scope.ServiceProvider.GetRequiredService<IZwiftPowerZoneConverter>();
        
        var profile = await zwiftClient.GetProfileAsync(stoppingToken);
        var configuration = await configProvider.GetConfigurationAsync(userId, stoppingToken);

        if (configuration.LightsOnActivityStart)
        {
            await hueClient.InitAsync(stoppingToken);
        }

        var delay = configuration.PowerRefreshMilliseconds;
        while (stoppingToken.IsCancellationRequested is false)
        {
            await Task.Delay(delay, stoppingToken);
            
            var (isScrapped, data) = await zwiftClient.GetActivityDataAsync(userId, stoppingToken);

            if (isScrapped is false)
            {
                continue;
            }

            var powerZoneColor = powerZoneConverter.ConvertPowerZoneColor(data.Power, profile.Ftp, configuration);

            if (powerZoneColor is null)
            {
                delay = configuration.PowerRefreshMilliseconds;
                continue;
            }

            delay = 5_000;
            await hueClient.SetLightsColorAsync(powerZoneColor.Hue, powerZoneColor.Xy, HueEffects.None, stoppingToken);
        }
    }
}