using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Attachments.Responses;
using SmartAttendance.Application.Features.Attendances.Responses;
using SmartAttendance.Application.Features.Excuses.Responses;
using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Common.Common.Responses.Users.Queries.Base;
using SmartAttendance.Common.General.Enums;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Common.Utilities.TypeConverters;
using SmartAttendance.Domain.Features.Attendances;
using SmartAttendance.Domain.Features.PlanEnrollments;
using SmartAttendance.Domain.Features.Plans;
using SmartAttendance.Persistence.Db;
using SmartAttendance.Persistence.Services.Identities;
using SmartAttendance.Persistence.Services.Taskes;

namespace SmartAttendance.RequestHandlers.Features.Plans.Queries.GetPlanByDateRange;

public class GetPlanEnrollmentsQueryHandler(
    IdentityService identityService,
    IPlanEnrollmentQueryRepository enrollmentQueryRepo   ,
    AutoMarkAbsentJob absentJob
) : IRequestHandler<GetPlanEnrollmentsQuery, List<GetEnrollmentResponse>>
{
    public async Task<List<GetEnrollmentResponse>> Handle(GetPlanEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        //await absentJob.RunForEndedPlans(request.PlanId , cancellationToken);

        var enrollments = await enrollmentQueryRepo.TableNoTracking
            .Include(e => e.Plan)
            .Include(a => a.Attendance)
            .ThenInclude(a => a.Excuse)
            .Include(a => a.Student)
            .Where(e => e.PlanId == request.PlanId)
            .Where(a => identityService.GetRoles() != Roles.Student || a.Student.Id == identityService.GetUserId<Guid>() )
            .Select(e => new GetEnrollmentResponse
            {
                PlanId = e.PlanId,
                Id =  e.Id,
                Status = e.Status,
                EnrolledAt =  e.EnrolledAt,
                Student = new GetUserResponse
                {
                    Id             = e.Student.Id,
                    FullName       = e.Student.FullName() ?? "دانشجو حذف شده",
                    ProfilePicture = e.Student.ProfilePicture != null
                        ? e.Student.ProfilePicture.BuildImageUrl(false)
                        : null,
                    FirstName = e.Student.FirstName ?? string.Empty,
                    LastName  = e.Student.LastName ?? string.Empty,
                },

                Start      = e.Plan.StartTime ,
                End        = e.Plan.EndTime ,
                Address    = e.Plan.Address ?? string.Empty,
                PlanName   = e.Plan.CourseName ?? "بدون عنوان",

                Attendance = e.Attendance != null
                    ? new GetAttendanceInfoResponse
                    {
                        Id      = e.Attendance.Id,
                        Student = new GetUserResponse
                        {
                            Id             = e.Student.Id,
                            FullName       = e.Student.FullName() ?? "نامشخص",
                            ProfilePicture = e.Student.ProfilePicture != null
                                ? e.Student.ProfilePicture.BuildImageUrl(false)
                                : null
                        },
                        Status     = e.Attendance.Status,
                        RecordedAt = e.Attendance.RecordedAt,

                        Excuse = e.Attendance.Excuse != null
                            ? new GetExcuseInfoResponse
                            {
                                Id          = e.Attendance.Excuse.Id,
                                Status      = e.Attendance.Excuse.Status,
                                SubmittedAt = e.Attendance.Excuse.SubmittedAt,
                                Reason      = e.Attendance.Excuse.Reason ?? string.Empty,

                                Attachment = e.Attendance.Excuse.Attachment != null
                                    ? new GetAttachmentInfoResponse
                                    {
                                        Id          = e.Attendance.Excuse.Attachment.Id,
                                        FileName    = e.Attendance.Excuse.Attachment.FileName ?? "Unknown",
                                        Url         = e.Attendance.Excuse.Attachment.Url ?? string.Empty,
                                        ContentType = e.Attendance.Excuse.Attachment.ContentType ?? "application/octet-stream",
                                        UploadedBy  = e.Attendance.Excuse.Attachment.UploadedBy,
                                        Uploader    = e.Attendance.Excuse.Attachment.Uploader != null
                                            ? new GetUserResponse
                                            {
                                                Id       = e.Attendance.Excuse.Attachment.Uploader.Id,
                                                FullName = e.Attendance.Excuse.Attachment.Uploader.FullName() ?? "سیستم"
                                            }
                                            : null
                                    }
                                    : null
                            }
                            : null
                    }
                    : null
            })
            .ToListAsync(cancellationToken);


        return enrollments;
    }
}

