namespace SmartAttendance.Application.Abstractions;

public interface IRoleRepository
{
    Task<bool> ExistsByNameAsync(string roleName, CancellationToken ct = default);
    Task       AddAsync(RoleId id, string name, CancellationToken ct = default);
}