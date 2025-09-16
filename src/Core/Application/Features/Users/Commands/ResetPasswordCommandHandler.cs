using SmartAttendance.Application.Abstractions.Identity;

namespace SmartAttendance.Application.Features.Users.Commands;

public sealed class ResetPasswordCommandHandler(
    IIdentityManagementService identity
) : IRequestHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        await identity.ResetPasswordAsync(request.Request.EmailOrUsername, request.Request.Token, request.Request.NewPassword, cancellationToken);
    }
}