using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Attachments.Responses;
using SmartAttendance.Application.Interfaces.Attachments;

namespace SmartAttendance.Application.Features.Attachments.Queries.GetByAttendanceId;

public class GetAttachmentsByAttendanceIdQueryHandler(
    IAttachmentQueryRepository queryRepository
) : IRequestHandler<GetAttachmentsByAttendanceIdQuery, List<GetAttachmentInfoResponse>>
{
    public async Task<List<GetAttachmentInfoResponse>> Handle(GetAttachmentsByAttendanceIdQuery request, CancellationToken cancellationToken)
    {
        var attachments = await queryRepository.TableNoTracking
            .Include(a => a.Uploader)
            .Where(a => a.RowId == request.AttendanceId)
            .ToListAsync(cancellationToken);

        return attachments.Adapt<List<GetAttachmentInfoResponse>>();
    }
}