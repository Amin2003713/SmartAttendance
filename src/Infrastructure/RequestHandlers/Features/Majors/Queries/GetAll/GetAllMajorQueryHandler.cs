using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Majors.Queries.GetAll;
using SmartAttendance.Application.Features.Majors.Responses;
using SmartAttendance.Application.Interfaces.Majors;

namespace SmartAttendance.RequestHandlers.Features.Majors.Queries.GetAll;

public class GetAllMajorQueryHandler(
    IMajorQueryRepository queryRepository
) : IRequestHandler<GetAllMajorQuery, List<GetMajorInfoResponse>>
{
    public async Task<List<GetMajorInfoResponse>> Handle(GetAllMajorQuery request, CancellationToken cancellationToken)
    {
        var majors = (await queryRepository.TableNoTracking.Include(a => a.Subjects)
                .OrderBy(m => m.Name)
                .ToListAsync(cancellationToken))
            .Adapt<List<GetMajorInfoResponse>>();


        return majors;
    }
}