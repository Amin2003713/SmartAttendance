namespace SmartAttendance.Application.Features.Users.Commands;

public sealed class DeleteUserCommandHandler(
    IUserRepository repo,
    IUnitOfWork uow
) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await repo.DeleteAsync(new UserId(request.UserId), cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
    }
}