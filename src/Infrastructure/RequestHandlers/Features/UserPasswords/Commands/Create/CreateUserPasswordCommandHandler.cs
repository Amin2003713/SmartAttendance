using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.UserPasswords.Commands.Create;
using SmartAttendance.Application.Interfaces.UserPasswords;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.RequestHandlers.Features.UserPasswords.Commands.Create;

public record CreateUserPasswordCommandHandler(
    UserManager<User>                                  UserManager,
    IUserPasswordQueryRepository                       PasswordQueryRepository,
    IUserPasswordCommandRepository                     PasswordCommandRepository,
    IPasswordHasher<User>                              Hasher,
    ILogger<CreateUserPasswordCommandHandler>          Logger,
    IStringLocalizer<CreateUserPasswordCommandHandler> Localizer
) : IRequestHandler<CreateUserPasswordCommand>
{
    public async Task Handle(CreateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Starting password creation for userId: {UserId}", request.UserId.Id);
        // Prevent using the same password as any previous one
        var previousPasswords = await PasswordQueryRepository.TableNoTracking.Where(a => a.UserId == request.UserId.Id).ToListAsync(cancellationToken);

        if (previousPasswords.Select(prev => Hasher.VerifyHashedPassword(request.UserId, prev.PasswordHash, request.Password))
            .Any(verifyResult => verifyResult == PasswordVerificationResult.Success))
        {
            Logger.LogWarning("User {UserId} tried to reuse an old password.", request.UserId);
            throw SmartAttendanceException.BadRequest(Localizer["Dont Reuse Old Password"]);
        }

        var hashedPass = Hasher.HashPassword(request.UserId, request.Password);

        var userPass = new UserPassword
        {
            UserId       = request.UserId.Id,
            PasswordHash = hashedPass
        };

        await PasswordCommandRepository.AddAsync(userPass, cancellationToken);

        Logger.LogInformation("Password for userId: {UserId} created successfully.", request.UserId);
    }
}