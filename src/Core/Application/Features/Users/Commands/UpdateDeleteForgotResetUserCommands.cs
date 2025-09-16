using SmartAttendance.Application.Abstractions.Identity;
using SmartAttendance.Application.Features.Users.Requests;
using SmartAttendance.Application.Features.Users.Responses;
using SmartAttendance.Domain.UserAggregate;

namespace SmartAttendance.Application.Features.Users.Commands;

// Update
public sealed record UpdateUserCommand(Guid UserId, UpdateUserRequest Request) : IRequest<UserProfileDto>;

public sealed class UpdateUserCommandHandler(IUserRepository repo, IUnitOfWork uow) : IRequestHandler<UpdateUserCommand, UserProfileDto>
{
	public async Task<UserProfileDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
	{
		var user = await repo.GetByIdAsync(new UserId(request.UserId), cancellationToken) ?? throw new KeyNotFoundException("کاربر یافت نشد.");
		user.UpdateContact(new EmailAddress(request.Request.Email), new PhoneNumber(request.Request.Phone));
		// نام‌ها
		var updated = new UserAggregate(user.Id, request.Request.FirstName, request.Request.LastName, user.Email, user.Phone, user.NationalCode);
		// حفظ نقش‌ها و وضعیت‌ها
		foreach (var r in user.Roles) updated.AssignRole(r);
		if (user.IsLocked) updated.RegisterFailedLogin(0);
		await repo.UpdateAsync(updated, cancellationToken);
		await uow.SaveChangesAsync(cancellationToken);
		return updated.Adapt<UserProfileDto>();
	}
}

// Delete
public sealed record DeleteUserCommand(Guid UserId) : IRequest;

public sealed class DeleteUserCommandHandler(IUserRepository repo, IUnitOfWork uow) : IRequestHandler<DeleteUserCommand>
{
	public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
	{
		await repo.DeleteAsync(new UserId(request.UserId), cancellationToken);
		await uow.SaveChangesAsync(cancellationToken);
	}
}

// Forgot Password
public sealed record ForgotPasswordCommand(ForgotPasswordRequest Request) : IRequest;

public sealed class ForgotPasswordCommandHandler(IIdentityManagementService identity) : IRequestHandler<ForgotPasswordCommand>
{
	public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
	{
		await identity.StartForgotPasswordAsync(request.Request.EmailOrUsername, cancellationToken);
	}
}

// Reset Password
public sealed record ResetPasswordCommand(ResetPasswordRequest Request) : IRequest;

public sealed class ResetPasswordCommandHandler(IIdentityManagementService identity) : IRequestHandler<ResetPasswordCommand>
{
	public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
	{
		await identity.ResetPasswordAsync(request.Request.EmailOrUsername, request.Request.Token, request.Request.NewPassword, cancellationToken);
	}
}

