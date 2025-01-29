using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shifty.Application.Shifts.Command.Create;
using Shifty.Common.Exceptions;
using Shifty.Domain.Features.Shifts;
using Shifty.Domain.Interfaces.Features.Shifts;
using Shifty.Persistence.Db;
using Shifty.Persistence.Repositories.Common;
using Shifty.Resources.Messages;

namespace Shifty.RequestHandlers.Shifts.Commands.CreateShift;

public class CreateShiftCommandHandler(
    IShiftCommandRepository command ,
    IShiftQueryRepository query ,
    ShiftMessages shiftMessages ,
    ILogger<CreateShiftCommandHandler> logger , ShiftMessages messages) : IRequestHandler<CreateShiftCommand>
{
    public async Task Handle(CreateShiftCommand request , CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            var shift = request.Adapt<Shift>();

            if (await query.Exist(shift , cancellationToken))
                throw ShiftyException.Conflict(messages.ALREADY_EXIST);


            await command.AddAsync(shift , cancellationToken);
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