using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Subjects.Queries.GetSubjectsByMajor;
using SmartAttendance.Application.Features.Subjects.Responses;
using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Subjects.Queries.GetSubjectsByMajor;

public class GetSubjectsByMajorQueryHandler(
    IdentityService  identityService,
    IMajorSubjectQueryRepository majorQueryRepository ,
    ISubjectQueryRepository queryRepository
) : IRequestHandler<GetSubjectsByMajorQuery , List<GetSubjectInfoResponse>>
{
    public async Task<List<GetSubjectInfoResponse>> Handle(GetSubjectsByMajorQuery request, CancellationToken cancellationToken)
    {
        var userRole = identityService.GetRoles();

        var subIds = await majorQueryRepository .TableNoTracking.Where(a => a.MajorId == request.MajorId)
            .Select(a => a.SubjectId)
            .ToListAsync(cancellationToken: cancellationToken);

        return await  queryRepository.TableNoTracking.Where(a => subIds.Contains(a.Id))
                   .ProjectToType<GetSubjectInfoResponse>()
                   .ToListAsync(cancellationToken) ??
               [];
    }
}