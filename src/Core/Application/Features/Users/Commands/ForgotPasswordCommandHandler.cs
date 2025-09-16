using SmartAttendance.Application.Abstractions.Identity;

namespace SmartAttendance.Application.Features.Users.Commands;

public sealed class ForgotPasswordCommandHandler(
    IIdentityManagementService identity
) : IRequestHandler<ForgotPasswordCommand>
{
    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        await identity.StartForgotPasswordAsync(request.Request.EmailOrUsername, cancellationToken);
    }
}