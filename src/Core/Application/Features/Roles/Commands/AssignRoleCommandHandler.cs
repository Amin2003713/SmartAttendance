namespace SmartAttendance.Application.Features.Roles.Commands;

public sealed class AssignRoleCommandHandler(
    IUserRepository userRepository,
    IRoleReadService roleReadService,
    IUnitOfWork unitOfWork
)
    : IRequestHandler<AssignRoleCommand>
{
    public async Task Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        var r    = request.Request;
        var user = await userRepository.GetByIdAsync(new UserId(r.UserId), cancellationToken) ?? throw new KeyNotFoundException("کاربر یافت نشد.");
        if (!await roleReadService.RoleExistsAsync(r.RoleName, cancellationToken))
            throw new KeyNotFoundException("نقش موردنظر معتبر نیست.");

        // در اینجا فقط نمونه‌ای از نقش را با شناسه تصادفی اضافه می‌کنیم
        user.AssignRole(RoleId.New());
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}