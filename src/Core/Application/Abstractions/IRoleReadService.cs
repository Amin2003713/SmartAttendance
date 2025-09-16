namespace SmartAttendance.Application.Abstractions;

public interface IRoleReadService
{
    Task<bool>                                    RoleExistsAsync(string roleName, CancellationToken ct = default);
    Task<(RoleId Id, string Name)?>               GetByIdAsync(RoleId id, CancellationToken ct = default);
    Task<IReadOnlyList<(RoleId Id, string Name)>> ListAsync(CancellationToken ct = default);
}