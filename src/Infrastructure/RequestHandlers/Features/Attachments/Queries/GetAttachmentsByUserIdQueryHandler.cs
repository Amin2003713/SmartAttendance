using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Attachments.Responses;
using SmartAttendance.Application.Interfaces.Attachments;

namespace SmartAttendance.Application.Features.Attachments.Queries.GetByAttendanceId;

public class GetAttachmentsByUserIdQueryHandler(
    IAttachmentQueryRepository queryRepository
) : IRequestHandler<GetAttachmentsByUserIdQuery, List<GetAttachmentInfoResponse>>
{
    public async Task<List<GetAttachmentInfoResponse>> Handle(GetAttachmentsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var attachments = await queryRepository.TableNoTracking
            .Include(a => a.Uploader)
            .Where(a => a.UploadedBy == request.UserId)
            .ToListAsync(cancellationToken);

        return attachments.Adapt<List<GetAttachmentInfoResponse>>();
    }
}