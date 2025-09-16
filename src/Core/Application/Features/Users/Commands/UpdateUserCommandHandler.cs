using SmartAttendance.Application.Features.Users.Responses;
using SmartAttendance.Domain.UserAggregate;

namespace SmartAttendance.Application.Features.Users.Commands;

public sealed class UpdateUserCommandHandler(
    IUserRepository repo,
    IUnitOfWork uow
) : IRequestHandler<UpdateUserCommand, UserProfileDto>
{
    public async Task<UserProfileDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await repo.GetByIdAsync(new UserId(request.UserId), cancellationToken) ?? throw new KeyNotFoundException("کاربر یافت نشد.");
        user.UpdateContact(new EmailAddress(request.Request.Email), new PhoneNumber(request.Request.Phone));
        // نام‌ها
        var updated = new UserAggregate(user.Id, request.Request.FirstName, request.Request.LastName, user.Email, user.Phone, user.NationalCode);

        // حفظ نقش‌ها و وضعیت‌ها
        foreach (var r in user.Roles)
        {
            updated.AssignRole(r);
        }

        if (user.IsLocked) updated.RegisterFailedLogin(0);
        await repo.UpdateAsync(updated, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        return updated.Adapt<UserProfileDto>();
    }
}