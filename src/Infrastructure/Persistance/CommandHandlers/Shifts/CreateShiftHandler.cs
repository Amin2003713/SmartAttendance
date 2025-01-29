using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Shifty.Application.Shifts.Command.Create;
using Shifty.Application.Shifts.Queries.GetDefault;
using Shifty.Common.Exceptions;
using Shifty.Domain.Features.Shifts;
using Shifty.Persistence.Db;
using Shifty.Persistence.Repositories.Common;
using Shifty.Resources.Messages;

namespace Shifty.Persistence.CommandHandlers.Shifts;

public class CreateShiftCommandHandler(
    Repository<Shift , WriteOnlyDbContext> repository ,
    ShiftMessages shiftMessages ,
    ILogger<CreateShiftCommandHandler> logger) : IRequestHandler<CreateShiftCommand>
{
    public async Task Handle(CreateShiftCommand request , CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            var shift = request.Adapt<Shift>();
            await repository.AddAsync(shift , cancellationToken);
        }
        catch (ShiftyException e)
        {
            logger.LogError(e.Source , e);
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occured while adding new shift.");
            throw ShiftyException.InternalServerError(shiftMessages.FAILED_TO_CREATE) ;
        }
    }
}



public class ListShiftsQueryHandler(Repository<Shift , ReadOnlyDbContext> repository , ShiftMessages shiftMessages , ILogger<ListShiftsQueryHandler> logger)
    : IRequestHandler<ListShiftsQuery , List<ListShiftsQueryResponse>>
{
    public Task<List<ListShiftsQueryResponse>> Handle(ListShiftsQuery request , CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return default;
    }
}