using Mapster;
using SmartAttendance.Application.Base.Settings.Queries.GetSetting;
using SmartAttendance.Application.Interfaces.Settings;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Base.Settings.Queries.GetSetting;

public class GetSettingQueryHandler(
    ISettingQueriesRepository                repository,
    ILogger<GetSettingQueryHandler>          logger,
    IStringLocalizer<GetSettingQueryHandler> localizer
) : IRequestHandler<GetSettingQuery, GetUniversitySettingResponse>
{
    public async Task<GetUniversitySettingResponse> Handle(GetSettingQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.GetSingleAsync(cancellationToken);

            if (result == null)
            {
                logger.LogWarning("No active price found.");
                throw SmartAttendanceException.NotFound(localizer["No Setting configuration found."]);
            }

            logger.LogInformation("Setting retrieved successfully. ID: {Id}", result.Id);

            return result.Adapt<GetUniversitySettingResponse>();
        }


        catch (SmartAttendanceException ex)
        {
            logger.LogError(ex, "Business exception occurred while retrieving Setting.");
            throw;
        }

        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while retrieving Setting.");
            throw SmartAttendanceException.InternalServerError(localizer["An unexpected error occurred while retrieving Setting."]);
        }
    }
}