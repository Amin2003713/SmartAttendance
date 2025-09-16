using SmartAttendance.Domain.PlanAggregate;

namespace SmartAttendance.Application.Abstractions;

public interface IPlanRepository
{
    Task<PlanAggregate?>               GetByIdAsync(PlanId id, CancellationToken ct = default);
    Task                               AddAsync(PlanAggregate plan, CancellationToken ct = default);
    Task                               UpdateAsync(PlanAggregate plan, CancellationToken ct = default);
    Task                               DeleteAsync(PlanId id, CancellationToken ct = default);
    Task<IReadOnlyList<PlanAggregate>> ListAsync(CancellationToken ct = default);
}