namespace ZwiftHue.Core.Infrastructure.Channels;

public interface IMessageChannel
{
    ValueTask SendAsync(ChannelMessage message, CancellationToken cancellationToken);
    IAsyncEnumerable<ChannelMessage> GetAsync(CancellationToken cancellationToken);
}