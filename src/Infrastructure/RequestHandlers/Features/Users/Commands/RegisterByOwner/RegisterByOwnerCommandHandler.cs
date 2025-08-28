using Microsoft.EntityFrameworkCore;
using Shifty.Application.Features.Users.Commands.AddRole;
using Shifty.Application.Features.Users.Commands.RegisterByOwner;
using Shifty.Application.Interfaces.Users;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Features.Users.Commands.RegisterByOwner;

public class RegisterByOwnerCommandHandler(
    IUserCommandRepository commandRepository,
    IUserQueryRepository queryRepository,
    IMediator mediator,
    ILogger<RegisterByOwnerCommandHandler> logger,
    IStringLocalizer<RegisterByOwnerCommandHandler> localizer
) : IRequestHandler<RegisterByOwnerCommand>
{
    public async Task Handle(RegisterByOwnerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (await queryRepository.TableNoTracking.AnyAsync(a => a.PhoneNumber == request.PhoneNumber,
                    cancellationToken))
            {
                logger.LogWarning("Duplicate phone number detected: {PhoneNumber}", request.PhoneNumber);
                throw ShiftyException.BadRequest(localizer["This phone number is already registered."].Value);
            }

            var userId = await commandRepository.RegisterByOwnerAsync(request, cancellationToken);


            await mediator.Send(new UpdateEmployeeCommand
                {
                    Roles = request.Roles.Select(a => a).ToList(),
                    UserId = userId
                },
                cancellationToken);

            logger.LogInformation("User with phone number {PhoneNumber} registered by owner successfully.",
                request.PhoneNumber);
        }
        catch (Exception e)
        {
            logger.LogError(e.Source, e);
            throw;
        }
    }
}