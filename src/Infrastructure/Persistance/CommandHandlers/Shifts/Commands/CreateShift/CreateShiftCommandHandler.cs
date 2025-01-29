using System;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Shifty.Application.Shifts.Command.Create;
using Shifty.Common.Exceptions;
using Shifty.Domain.Features.Shifts;
using Shifty.Persistence.Db;
using Shifty.Persistence.Repositories.Common;
using Shifty.Resources.Messages;

namespace Shifty.Persistence.CommandHandlers.Shifts.Commands.CreateShift;

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