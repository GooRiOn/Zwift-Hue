using System.Text.Json;
using ZwiftHue.Core.Infrastructure.Channels;

namespace ZwiftHue.Core.Commands.StartActivity;

internal sealed class StartActivityHandler : ICommandHandler<StartActivity>
{
    private readonly IMessageChannel _channel;

    public StartActivityHandler(IMessageChannel channel)
        => _channel = channel;

    public async Task HandleAsync(StartActivity command, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(command);
        var message = new ChannelMessage(typeof(StartActivity), json);
        await _channel.SendAsync(message, cancellationToken);
    }
}