using SmartAttendance.Application.Features.Users.Requests;
using SmartAttendance.Application.Features.Users.Responses;
using SmartAttendance.Domain.UserAggregate;

namespace SmartAttendance.Application.Features.Users.Commands;

// Command: ایجاد کاربر جدید
public sealed record CreateUserCommand(CreateUserRequest Request) : IRequest<UserProfileDto>;

// Handler: ایجاد کاربر جدید
public sealed class CreateUserCommandHandler(
	IUserRepository userRepository,
	IUnitOfWork unitOfWork
) : IRequestHandler<CreateUserCommand, UserProfileDto>
{
	public async Task<UserProfileDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
	{
		var r = request.Request;
		var user = new UserAggregate(
			UserId.New(),
			r.FirstName,
			r.LastName,
			new EmailAddress(r.Email),
			new PhoneNumber(r.Phone),
			new NationalCode(r.NationalCode)
		);

		await userRepository.AddAsync(user, cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);

		return user.Adapt<UserProfileDto>();
	}
}

