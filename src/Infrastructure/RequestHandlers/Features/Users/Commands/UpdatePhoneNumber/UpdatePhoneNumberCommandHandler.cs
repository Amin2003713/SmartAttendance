using Microsoft.AspNetCore.Identity;
using SmartAttendance.Application.Features.Users.Commands.UpdatePhoneNumber;
using SmartAttendance.Application.Interfaces.Users;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General;
using SmartAttendance.Domain.Users;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Users.Commands.UpdatePhoneNumber;

public class UpdatePhoneNumberCommandHandler(
    IUserCommandRepository                            commandRepository,
    UserManager<User>                                 userManager,
    IdentityService                                   identityService,
    IUserQueryRepository                              userRepository,
    ILogger<UpdatePhoneNumberCommandHandler>          logger,
    IStringLocalizer<UpdatePhoneNumberCommandHandler> localizer
) : IRequestHandler<UpdatePhoneNumberCommand>
{
    public async Task Handle(UpdatePhoneNumberCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = identityService.GetUserId<Guid>();

            var user = await userRepository.GetSingleAsync(cancellationToken,
                                                           a => a.Id == userId);

            if (user == null)
                throw SmartAttendanceException.NotFound(localizer["User was not found."]);

            user.PhoneNumber = request.PhoneNumber;

            var isValid = await userManager.VerifyTwoFactorTokenAsync(
                user,
                ApplicationConstant.Identity.CodeGenerator,
                request.Code);

            if (!isValid)
            {
                logger.LogWarning("Invalid code entered for phone number update: {Phone}", request.PhoneNumber);
                throw SmartAttendanceException.BadRequest(localizer["Invalid or expired activation code."]);
            }

            await commandRepository.UpdatePhoneNumberAsync(request, userId, cancellationToken);

            logger.LogInformation("Phone number updated successfully for user {UserId}", userId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating phone number for user.");
            throw;
        }
    }
}