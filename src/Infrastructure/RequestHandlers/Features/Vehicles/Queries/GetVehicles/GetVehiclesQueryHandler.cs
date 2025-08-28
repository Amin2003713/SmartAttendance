using System.Diagnostics;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shifty.Application.Features.Vehicles.Queries.GetVehicles;
using Shifty.Application.Features.Vehicles.Requests.Queries.GetVehicles;
using Shifty.Application.Interfaces.Vehicles;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Features.Vehicles.Queries.GetVehicles;

public class GetVehiclesQueryHandler(
    IVehicleQueryRepository queryRepository,
    ILogger<GetVehiclesQueryHandler> logger,
    IStringLocalizer<GetVehiclesQueryHandler> localizer)
    : IRequestHandler<GetVehiclesQuery, List<GetVehicleQueryResponse>>
{
    public async Task<List<GetVehicleQueryResponse>> Handle(
        GetVehiclesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicles = await queryRepository
                .TableNoTracking
                .ProjectToType<GetVehicleQueryResponse>()
                .ToListAsync(cancellationToken);

            if (vehicles.Count == 0)
            {   
                logger.LogInformation(localizer["NoVehiclesFound"]);
                return vehicles;
            }

            logger.LogInformation("{Message} {@Count}",
                localizer["VehiclesFound", vehicles.Count],
                vehicles.Count);

            return vehicles;
        }
        catch (ShiftyException ex)
        {
            logger.LogWarning(ex, localizer["BusinessErrorWhileRetrieving"]);
            throw; 
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation(localizer["OperationCanceled"]);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, localizer["UnexpectedErrorWhileRetrieving"]);
            throw;
        }
    }
}
