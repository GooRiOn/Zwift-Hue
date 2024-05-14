using ZwiftHue.Core.Infrastructure.ProfileConfigs;
using ZwiftHue.Core.Infrastructure.Zwift;

namespace ZwiftHue.Core.Queries.GetRiderProfile;

public class GetRiderProfileHandler(ZwiftClient zwiftClient, IRiderProfileConfigurationProvider provider) : IQueryHandler<GetRiderProfile, RiderProfileDto>
{
    public async Task<RiderProfileDto> HandleAsync(GetRiderProfile query, CancellationToken cancellationToken)
    {
        var zwiftProfile = await zwiftClient.GetProfileAsync(cancellationToken);
        var configuration = await provider.GetConfigurationAsync(zwiftProfile.Id, cancellationToken);

        return new RiderProfileDto
        {
            Id = zwiftProfile.Id,
            FirstName = zwiftProfile.FirstName,
            LastName = zwiftProfile.LastName,
            ImageSrc = zwiftProfile.ImageSrc,
            Age = zwiftProfile.Age,
            Ftp = zwiftProfile.Ftp,
            Configuration = new RiderProfileConfigurationDto
            {
                LightsOnActivityStart = configuration.LightsOnActivityStart,
                PowerZoneMissRatio = configuration.PowerZoneMissRatio
            }
        };
    }
}