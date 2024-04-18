using ZwiftHue.Core;
using ZwiftHue.Core.Hue;
using ZwiftHue.Core.Zwift;

var builder = WebApplication.CreateBuilder(args);

    builder.Services
    .AddZwift(builder.Configuration)
    .AddHue(builder.Configuration)
    .AddHostedService<Worker>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();