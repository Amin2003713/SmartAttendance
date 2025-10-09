using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Majors.Queries.GetById;
using SmartAttendance.Application.Features.Majors.Responses;
using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Features.Majors.Queries.GetById;

public class GetMajorByIdHandler(
    IMajorQueryRepository queryRepository
) : IRequestHandler<GetMajorById, GetMajorInfoResponse>
{
    public async Task<GetMajorInfoResponse> Handle(GetMajorById request, CancellationToken cancellationToken)
    {
        var major = await queryRepository.TableNoTracking.ProjectToType<GetMajorInfoResponse>()
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        return major ?? throw SmartAttendanceException.NotFound("Major not found");
    }
}