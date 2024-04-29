using ZwiftHue.Core.Exceptions;
using ZwiftHue.Core.Infrastructure.Zwift;

namespace ZwiftHue.Core.Commands.Login;

internal sealed class LoginRiderHandler(ZwiftClient zwiftClient) : ICommandHandler<LoginRider>
{
    public async Task HandleAsync(LoginRider command, CancellationToken cancellationToken)
    {
        var (email, password) = command;
        var isSucceeded = await zwiftClient.AuthenticateAsync(email, password, cancellationToken);

        if (isSucceeded is false)
        {
            throw new ZwiftHueException("Invalid credentials");
        }
    }
}