using System.Text.Json;

namespace ZwiftHue.Core.Infrastructure.ProfileConfigs;

internal sealed class RiderProfileConfigurationProvider : IRiderProfileConfigurationProvider
{
    private const string Path = "../ZwiftHue.Core/Infrastructure/ProfileConfigs/profiles.json";
    
    public async Task<RiderProfileConfiguration> GetConfigurationAsync(int userId, CancellationToken cancellationToken)
    {
        var json = await File.ReadAllTextAsync(Path, cancellationToken);
        var profileConfigs = JsonSerializer.Deserialize<RiderProfileConfiguration[]>(json);

        var selectedProfileConfig = profileConfigs?.SingleOrDefault(x => x.Id == userId);
        return selectedProfileConfig;
    }
}