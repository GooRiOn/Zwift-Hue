using Microsoft.AspNetCore.Mvc;
using ZwiftHue.Api.Middlewares;
using ZwiftHue.Core;
using ZwiftHue.Core.Commands;
using ZwiftHue.Core.Commands.Login;
using ZwiftHue.Core.Commands.StartActivity;
using ZwiftHue.Core.Infrastructure.Zwift.DTO;
using ZwiftHue.Core.Queries;
using ZwiftHue.Core.Queries.GetRiderProfile;

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
app.MapGet("/me", (IQueryHandler<GetRiderProfile, RiderProfileDto> handler, CancellationToken cancellationToken) =>
    handler.HandleAsync(new GetRiderProfile(), cancellationToken));
app.MapPost("/login", (LoginRider command, ICommandHandler<LoginRider> handler, CancellationToken cancellationToken) =>
    handler.HandleAsync(command, cancellationToken));
app.MapPost("/activity/{userId:int}/start", ([FromRoute] int userId, ICommandHandler<StartActivity> handler, CancellationToken cancellationToken) =>
    handler.HandleAsync(new StartActivity(userId), cancellationToken));

app.Run();