using ZwiftHue.Api.Middlewares;
using ZwiftHue.Core;
using ZwiftHue.Core.Commands;
using ZwiftHue.Core.Commands.Login;
using ZwiftHue.Core.Hue;
using ZwiftHue.Core.Zwift;

var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddCore(builder.Configuration)
        .AddSingleton<ErrorMiddleware>();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "Policy",
            policy  =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();;
            });
    });

var app = builder.Build();

app.UseMiddleware<ErrorMiddleware>();
app.UseCors("Policy");

app.MapGet("/", () => "Zwift Hue API");
app.MapPost("/login", (LoginRider command, ICommandHandler<LoginRider> handler, CancellationToken cancellationToken) =>
    handler.HandleAsync(command, cancellationToken));

app.Run();