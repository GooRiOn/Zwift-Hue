using ZwiftHue;
using ZwiftHue.Zwift;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddZwift(builder.Configuration);

var host = builder.Build();
host.Run();