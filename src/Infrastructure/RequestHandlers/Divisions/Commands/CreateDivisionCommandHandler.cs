using System.Net;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Shifty.Application.Divisions.Command.Create;
using Shifty.Common.Exceptions;
using Shifty.Domain.Features.Divisions;
using Shifty.Domain.Features.Shifts;
using Shifty.Domain.Interfaces.Features.Divisions.Commands;
using Shifty.Resources.Messages;

namespace Shifty.RequestHandlers.Divisions;

public class CreateDivisionCommandHandler(IDivisionCommandRepository repository , DivisionMessages messages , ILogger<CreateDivisionCommandHandler> logger)
    : IRequestHandler<CreateDivisionCommand>
{
    public async Task Handle(CreateDivisionCommand request , CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            var division = request.Adapt<Division>();

            if (division == null)
                ArgumentNullException.ThrowIfNull(division);

            if (await repository.Exist(division , cancellationToken))
                throw ShiftyException.Conflict(messages.ALREADY_EXIST);

            await repository.AddAsync(division , cancellationToken);
        }
        catch (ShiftyException e)
        {
            logger.LogError(e , e.Message);
            throw;
        }
        catch (Exception e)
        {
logger.LogError(e, e.Message);
throw ShiftyException.Create(HttpStatusCode.InternalServerError , messages.FAILED_TO_CREATE);
        }
    }
}