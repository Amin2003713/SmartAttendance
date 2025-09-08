using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Users.Queries.GetAllUsers;
using SmartAttendance.Application.Features.Vehicles.Queries.GetById;
using SmartAttendance.Application.Features.Vehicles.Requests.Queries.GetVehicles;
using SmartAttendance.Application.Interfaces.Vehicles;
using SmartAttendance.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;

namespace SmartAttendance.RequestHandlers.Features.Vehicles.Queries.GetById;

public class GetVehicleByIdQueryHandler(
    IVehicleQueryRepository queryRepository,
    IMediator mediator,
    ILogger<GetVehicleByIdQueryHandler> logger
) : IRequestHandler<GetVehicleByIdQuery, GetVehicleQueryResponse>
{
    public async Task<GetVehicleQueryResponse> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await queryRepository.TableNoTracking
            .Where(x => x.Id == request.Id)
            .ProjectToType<GetVehicleQueryResponse>()
            .FirstOrDefaultAsync(cancellationToken);

        if (vehicle is null)
            return new GetVehicleQueryResponse();

        var users          = await mediator.Send(new GetAllUsersQuery(), cancellationToken);
        var userDictionary = users.ToDictionary(u => u.Id);

        var rawVehicle = await queryRepository.TableNoTracking
            .Where(x => x.Id == request.Id)
            .Select(x => new
            {
                x.ResponsibleId
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (rawVehicle is not null && userDictionary.TryGetValue(rawVehicle.ResponsibleId, out var user))
        {
            vehicle.Responsible = user.Adapt<LogPropertyInfoResponse>();
        }
        else
        {
            logger.LogWarning("User with ID {ResponsibleId} not found.", rawVehicle?.ResponsibleId);
            vehicle.Responsible = null!;
        }

        return vehicle;
    }
}