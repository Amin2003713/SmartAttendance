using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Subjects.Queries.GetTeacherSubjects;
using SmartAttendance.Application.Features.Subjects.Responses;
using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Subjects.Queries.GetTeacherSubjects;

public class GetTeacherSubjectsQueryHandler(
    IdentityService  identityService,
    ISubjectTeacherQueryRepository teachQueryRepository ,
    ISubjectQueryRepository queryRepository
) : IRequestHandler<GetTeacherSubjectsQuery , List<GetSubjectInfoResponse>>
{
    public async Task<List<GetSubjectInfoResponse>> Handle(GetTeacherSubjectsQuery request, CancellationToken cancellationToken)
    {
        var userRole = identityService.GetRoles();

        var subIds = await teachQueryRepository.TableNoTracking.Where(a => a.TeacherId == request.TeacherId)
            .Select(a => a.SubjectId)
            .ToListAsync(cancellationToken: cancellationToken);

        return await  queryRepository.TableNoTracking.Where(a => subIds.Contains(a.Id))
                   .ProjectToType<GetSubjectInfoResponse>()
                   .ToListAsync(cancellationToken) ??
               [];
    }
}