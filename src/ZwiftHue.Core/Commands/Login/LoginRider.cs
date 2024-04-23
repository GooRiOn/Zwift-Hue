namespace ZwiftHue.Core.Commands.Login;

public record LoginRider(string Email, string Password) : ICommand;