using ZwiftHue;
using ZwiftHue.Hue;
using ZwiftHue.Zwift;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddZwift(builder.Configuration)
    .AddHue(builder.Configuration)
    .AddHostedService<Worker>();

var host = builder.Build();
host.Run();