using Microsoft.AspNetCore.Identity;
using Shifty.Application.Features.Users.Commands.UpdatePhoneNumber;
using Shifty.Application.Interfaces.Users;
using Shifty.Common.Exceptions;
using Shifty.Common.General;
using Shifty.Domain.Users;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.Users.Commands.UpdatePhoneNumber;

public class UpdatePhoneNumberCommandHandler(
    IUserCommandRepository commandRepository,
    UserManager<User> userManager,
    IdentityService identityService,
    IUserQueryRepository userRepository,
    ILogger<UpdatePhoneNumberCommandHandler> logger,
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
                throw ShiftyException.NotFound(localizer["User was not found."]);

            user.PhoneNumber = request.PhoneNumber;

            var isValid = await userManager.VerifyTwoFactorTokenAsync(
                user,
                ApplicationConstant.Identity.CodeGenerator,
                request.Code);

            if (!isValid)
            {
                logger.LogWarning("Invalid code entered for phone number update: {Phone}", request.PhoneNumber);
                throw ShiftyException.BadRequest(localizer["Invalid or expired activation code."]);
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