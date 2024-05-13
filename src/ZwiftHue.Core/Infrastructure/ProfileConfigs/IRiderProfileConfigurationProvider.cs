namespace ZwiftHue.Core.Infrastructure.ProfileConfigs;

public interface IRiderProfileConfigurationProvider
{
    Task<RiderProfileConfiguration> GetConfigurationAsync(int userId, CancellationToken cancellationToken);
}