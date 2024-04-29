using ZwiftHue.Core.Infrastructure.Zwift;
using ZwiftHue.Core.Infrastructure.Zwift.DTO;

namespace ZwiftHue.Core.Queries.GetProfile;

public class GetProfileHandler(ZwiftClient zwiftClient) : IQueryHandler<GetProfile, ZwiftProfileDto>
{
    public Task<ZwiftProfileDto> HandleAsync(GetProfile query, CancellationToken cancellationToken)
        => zwiftClient.GetProfileAsync(cancellationToken);
}