using SmartAttendance.Application.Features.Plans.Responses;

namespace SmartAttendance.Application.Features.Plans.Queries;

// Query: فهرست طرح‌ها
public sealed record ListPlansQuery() : IRequest<IReadOnlyList<PlanDto>>;

public sealed class ListPlansQueryHandler(IPlanRepository repo) : IRequestHandler<ListPlansQuery, IReadOnlyList<PlanDto>>
{
	public async Task<IReadOnlyList<PlanDto>> Handle(ListPlansQuery request, CancellationToken cancellationToken)
	{
		var list = await repo.ListAsync(cancellationToken);
		return list.Adapt<IReadOnlyList<PlanDto>>();
	}
}

