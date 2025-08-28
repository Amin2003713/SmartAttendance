using Mapster;
using Shifty.Application.Base.Settings.Queries.GetSetting;
using Shifty.Application.Interfaces.Settings;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Base.Settings.Queries.GetSetting;

public class GetSettingQueryHandler(
    ISettingQueriesRepository repository,
    ILogger<GetSettingQueryHandler> logger,
    IStringLocalizer<GetSettingQueryHandler> localizer
) : IRequestHandler<GetSettingQuery, GetSettingQueryResponse>
{
    public async Task<GetSettingQueryResponse> Handle(GetSettingQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.GetSingleAsync(cancellationToken);

            if (result == null)
            {
                logger.LogWarning("No active price found.");
                throw ShiftyException.NotFound(localizer["No Setting configuration found."]);
            }

            logger.LogInformation("Setting retrieved successfully. ID: {Id}", result.Id);

            return result.Adapt<GetSettingQueryResponse>();
        }


        catch (ShiftyException ex)
        {
            logger.LogError(ex, "Business exception occurred while retrieving Setting.");
            throw;
        }

        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while retrieving Setting.");
            throw ShiftyException.InternalServerError(localizer["An unexpected error occurred while retrieving Setting."]);
        }
    }
}