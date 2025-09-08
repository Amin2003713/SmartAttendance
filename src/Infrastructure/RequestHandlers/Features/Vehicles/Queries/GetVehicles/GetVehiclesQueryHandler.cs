using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Users.Queries.GetAllUsers;
using SmartAttendance.Application.Features.Vehicles.Queries.GetVehicles;
using SmartAttendance.Application.Features.Vehicles.Requests.Queries.GetVehicles;
using SmartAttendance.Application.Interfaces.Vehicles;
using SmartAttendance.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Features.Vehicles.Queries.GetVehicles;

public class GetVehiclesQueryHandler(
    IVehicleQueryRepository queryRepository,
    IMediator mediator,
    ILogger<GetVehiclesQueryHandler> logger,
    IStringLocalizer<GetVehiclesQueryHandler> localizer
)
    : IRequestHandler<GetVehiclesQuery, List<GetVehicleQueryResponse>>
{
    public async Task<List<GetVehicleQueryResponse>> Handle(
        GetVehiclesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var rawData = await queryRepository.TableNoTracking
                .Select(x => new
                {
                    Vehicle = x.Adapt<GetVehicleQueryResponse>(),
                    x.ResponsibleId
                })
                .ToListAsync(cancellationToken);

            if (rawData.Count == 0)
            {
                logger.LogInformation(localizer["NoVehiclesFound"]);
                return [];
            }

            var users          = await mediator.Send(new GetAllUsersQuery(), cancellationToken);
            var userDictionary = users.ToDictionary(u => u.Id);

            foreach (var entry in rawData)
            {
                if (userDictionary.TryGetValue(entry.ResponsibleId, out var user))
                {
                    entry.Vehicle.Responsible = user.Adapt<LogPropertyInfoResponse>();
                }
            }

            var vehicles = rawData.Select(x => x.Vehicle).ToList();

            logger.LogInformation("{Message} {@Count}",
                localizer["VehiclesFound", vehicles.Count],
                vehicles.Count);

            return vehicles;
        }
        catch (SmartAttendanceException ex)
        {
            logger.LogWarning(ex, localizer["BusinessErrorWhileRetrieving"]);
            throw;
        }

        catch (Exception ex)
        {
            logger.LogError(ex, localizer["UnexpectedErrorWhileRetrieving"]);
            throw;
        }
    }
}