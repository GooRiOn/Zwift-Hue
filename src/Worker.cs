namespace ZwiftHue;
using Monitor = ZwiftPacketMonitor.Monitor;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly Monitor _monitor;

    public Worker(ILogger<Worker> logger, Monitor monitor)
    {
        _logger = logger;
        _monitor = monitor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _monitor.IncomingPlayerEvent += (s, e) => {
            _logger.LogInformation($"INCOMING: {e.PlayerState}");
        };
        _monitor.OutgoingPlayerEvent += (s, e) => {
            _logger.LogInformation($"OUTGOING: {e.PlayerState}");
        };
        _monitor.IncomingChatMessageEvent += (s, e) => {
            _logger.LogInformation($"CHAT: {e.Message}");
        };
        _monitor.IncomingPlayerEnteredWorldEvent += (s, e) => {
            _logger.LogInformation($"WORLD: {e.PlayerUpdate}");
        };
        _monitor.IncomingRideOnGivenEvent += (s, e) => {
            _logger.LogInformation($"RIDEON: {e.RideOn}");
        };

        await _monitor.StartCaptureAsync("en0", stoppingToken);
    }
}