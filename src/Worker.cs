using Grpc.Net.Client;
using ZwiftHue;
using ZwiftHue.Zwift;

namespace ZwiftHue;

public class Worker : BackgroundService
{
    private readonly ZwiftClient _zwiftClient;
    private readonly ILogger<Worker> _logger;

    public Worker(ZwiftClient zwiftClient, ILogger<Worker> logger)
    {
        _zwiftClient = zwiftClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            Console.WriteLine("Zwift username");
            var username = Console.ReadLine();
        
            Console.WriteLine("Zwift password");
            var password = Console.ReadLine();
            
            Console.WriteLine("Zwift ID");
            var zwiftId = Console.ReadLine();

            var isSucceeded = await _zwiftClient.AuthenticateAsync(username, password, stoppingToken);

            if (isSucceeded)
            {
                break;
            }
            
            Console.WriteLine("LOGIN FAILED. Press any key to retry...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}