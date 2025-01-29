using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shifty.Application.Shifts.Queries.GetDefault;
using Shifty.Common.Exceptions;
using Shifty.Domain.Features.Shifts;
using Shifty.Domain.Interfaces.Base;
using Shifty.Persistence.Db;
using Shifty.Persistence.Repositories.Common;
using Shifty.Resources.Messages;

namespace Shifty.Persistence.CommandHandlers.Shifts.Queries.ListShifts;

public class ListShiftsQueryHandler(IRepository<Shift> repository , ShiftMessages shiftMessages , ILogger<ListShiftsQueryHandler> logger)
    : IRequestHandler<ListShiftsQuery , List<ListShiftsQueryResponse>>
{
    public async Task<List<ListShiftsQueryResponse>> Handle(ListShiftsQuery request , CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            return (await repository.TableNoTracking.ToListAsync(cancellationToken: cancellationToken)).Adapt<List<ListShiftsQueryResponse>>();
        }
        catch (ShiftyException e)
        {
            logger.LogError(e.Source , e);
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e , "An error occured while adding new shift.");
            throw ShiftyException.InternalServerError(shiftMessages.FAILED_TO_CREATE);
        }

        return default;
    }
}