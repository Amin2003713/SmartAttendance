using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Users.Commands.AddRole;
using SmartAttendance.Application.Features.Users.Commands.RegisterByOwner;
using SmartAttendance.Application.Interfaces.Users;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Features.Users.Commands.RegisterByOwner;

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
                throw SmartAttendanceException.BadRequest(localizer["This phone number is already registered."].Value);
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