using ZwiftHue;
using ZwiftPacketMonitor;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddZwiftPacketMonitoring();

var host = builder.Build();
host.Run();