using System.Threading.Channels;

namespace ZwiftHue.Core.Infrastructure.Channels;

internal sealed class MessageChannel : IMessageChannel
{
    private readonly Channel<ChannelMessage> _channel = Channel.CreateUnbounded<ChannelMessage>();

    public ValueTask SendAsync(ChannelMessage message, CancellationToken cancellationToken)
        => _channel.Writer.WriteAsync(message, cancellationToken);

    public IAsyncEnumerable<ChannelMessage> GetAsync(CancellationToken cancellationToken)
        => _channel.Reader.ReadAllAsync(cancellationToken);
}