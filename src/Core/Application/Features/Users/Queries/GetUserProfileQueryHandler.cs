using SmartAttendance.Application.Features.Users.Responses;

namespace SmartAttendance.Application.Features.Users.Queries;

public sealed class GetUserProfileQueryHandler(
    IUserRepository userRepository
) : IRequestHandler<GetUserProfileQuery, UserProfileDto>
{
    public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(new UserId(request.UserId), cancellationToken) ?? throw new KeyNotFoundException("کاربر مورد نظر یافت نشد.");
        return user.Adapt<UserProfileDto>();
    }
}