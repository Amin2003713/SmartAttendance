namespace SmartAttendance.Application.Features.Roles.Commands;

public sealed class CreateRoleCommandHandler(
    IRoleRepository repo,
    IUnitOfWork uow
) : IRequestHandler<CreateRoleCommand>
{
    public async Task Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var name = request.Request.Name?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("نام نقش الزامی است.");
        if (await repo.ExistsByNameAsync(name, cancellationToken)) throw new InvalidOperationException("نام نقش تکراری است.");

        await repo.AddAsync(RoleId.New(), name, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
    }
}