using System.Linq;
using System.Reflection;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SmartAttendance.Application.Base.Universities.Commands.InitialUniversity;
using SmartAttendance.Application.Base.Universities.Responses.GetCompanyInfo;
using SmartAttendance.Application.Features.Attachments.Responses;
using SmartAttendance.Application.Features.Attendances.Responses;
using SmartAttendance.Application.Features.Excuses.Responses;
using SmartAttendance.Application.Features.Majors.Responses;
using SmartAttendance.Application.Features.Plans.Responses;
using SmartAttendance.Application.Features.Subjects.Responses;
using SmartAttendance.Application.Features.Users.Commands.UpdateUser;
using SmartAttendance.Application.Features.Users.Queries.GetUserTenants;
using SmartAttendance.Application.Features.Users.Requests.Queries.GetUserInfo.GetById;
using SmartAttendance.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;
using SmartAttendance.Common.Common.Responses.Users.Queries.Base;
using SmartAttendance.Common.General.Enums.Plans.Enrollment;
using SmartAttendance.Common.Utilities.TypeConverters;
using SmartAttendance.Domain.Features.Attendances;
using SmartAttendance.Domain.Features.Excuses;
using SmartAttendance.Domain.Features.Majors;
using SmartAttendance.Domain.Features.PlanEnrollments;
using SmartAttendance.Domain.Features.Plans;
using SmartAttendance.Domain.Features.Subjects;
using SmartAttendance.Domain.Tenants;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        services.AddCustomFluentValidation();
        ConfigureMaster();
        return services;
    }

    private static void AddCustomFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private static void ConfigureMaster()
    {
        OtherAdaptor();
        UserAdaptor();
        PlanAdaptor();
        MajorAdaptor();
        SubjectAdaptor();
        EnrollmentAdaptor();
        AttendanceAdaptor();
        ExcuseAdaptor();
    }

    private static void ExcuseAdaptor()
    {
        TypeAdapterConfig<Excuse, GetExcuseInfoResponse>.NewConfig()
            .Map(dest => dest.Attachment,
                src => src.Attachment.Adapt<GetAttachmentInfoResponse>()
            );
    }

    private static void AttendanceAdaptor()
    {
        TypeAdapterConfig<Attendance, GetAttendanceInfoResponse>.NewConfig()
            .Map(dest => dest.Student,
                src => src.Enrollment.Student.Adapt<GetAttendanceInfoResponse>()
            )
            .Map(dest => dest.Excuse,
                src => src.Excuse.Adapt<GetExcuseInfoResponse>()
            )
            .Map(dest => dest.Excuse,
                src => src.Excuse.Adapt<GetExcuseInfoResponse>()
            );
    }

    private static void EnrollmentAdaptor()
    {
        TypeAdapterConfig<PlanEnrollment, GetEnrollmentResponse>.NewConfig()
            .Map(dest => dest.Attendance,
                e => new GetEnrollmentResponse
                {
                    PlanId = e.PlanId,
                    Student = e.Student == null
                        ? null
                        : new GetUserResponse
                        {
                            Id = e.Student.Id,
                            FullName = e.Student.FullName(),
                            ProfilePicture = e.Student.ProfilePicture.BuildImageUrl(false)
                        },
                    Attendance = e.Attendance == null
                        ? null
                        : new GetAttendanceInfoResponse
                        {
                            Id = e.Attendance.Id,
                            RecordedAt = e.Attendance.RecordedAt,
                            Excuse = e.Attendance.Excuse == null
                                ? null
                                : new GetExcuseInfoResponse
                                {
                                    Id = e.Attendance.Excuse.Id,
                                    Reason = e.Attendance.Excuse.Reason
                                }
                        },
                    Address = e.Plan.Address,
                    End = e.Plan.EndTime,
                    PlanName = e.Plan.CourseName,
                    Start = e.Plan.StartTime,
                    Status = e.Status,
                    EnrolledAt = e.EnrolledAt
                });
    }

    private static void SubjectAdaptor() { }

    private static void MajorAdaptor()
    {
        TypeAdapterConfig<Major, GetMajorInfoResponse>.NewConfig()
            .Map(dest => dest.Subjects,
                src => src.Subjects != null && src.Subjects.Any()
                    ? src.Subjects.Select(s => new GetSubjectInfoResponse
                        {
                            Id = s.Id,
                            Name = s.Name,
                        })
                        .ToList()
                    : null
            );
    }


private static void PlanAdaptor()
{
    TypeAdapterConfig<Plan, GetPlanInfoResponse>.NewConfig()

        // Major
        .Map(dest => dest.Major,
            src => src.Major != null
                ? new GetMajorInfoResponse
                {
                    Id = src.Major.Id,
                    Name = src.Major.Name ?? string.Empty,
                    HeadMasterId = src.Major.HeadMasterId,
                    Subjects = src.Major.Subjects
                        .Where(s => s != null)
                        .Select(s => new MajorSubjectResponse(s.Id, s.Name ?? string.Empty))
                        .ToList() ?? new List<MajorSubjectResponse>()
                }
                : null)

        // Teachers
        .Map(dest => dest.Teacher,
            src => src.Teacher
                .Where(t => t.Teacher != null)
                .Select(t => new GetUserResponse
                {
                    Id = t.Teacher.Id,
                    FullName = t.Teacher.FullName() ?? "نامشخص",
                    ProfilePicture = t.Teacher.ProfilePicture.BuildImageUrl(false),
                    FirstName = t.Teacher.FirstName ?? string.Empty,
                    LastName = t.Teacher.LastName ?? string.Empty,
                    UserName = t.Teacher.UserName ?? string.Empty
                    // بقیه فیلدها رو هم می‌تونی اضافه کنی اگر نیازه
                })
                .ToList() ?? new List<GetUserResponse>())

        // Enrollments (مهم‌ترین قسمت — خیلی حساس به null)
        .Map(dest => dest.Enrollments,
            src => src.Enrollments
                .Where(e => e != null && e.Student != null)
                .Select(e => new GetEnrollmentResponse
                {
                    PlanId = e.PlanId,
                    Student = new GetUserResponse
                    {
                        Id = e.Student.Id,
                        FullName = e.Student.FullName() ?? "دانشجو حذف شده",
                        ProfilePicture = e.Student.ProfilePicture!.BuildImageUrl(false),
                        FirstName = e.Student.FirstName ?? string.Empty,
                        LastName = e.Student.LastName ?? string.Empty,
                    },
                    Status = e.Status,
                    EnrolledAt = e.EnrolledAt,
                    Start = e.Plan.StartTime,
                    End = e.Plan.EndTime,
                    Address = e.Plan.Address ?? string.Empty,
                    PlanName = e.Plan.CourseName ?? "بدون عنوان",

                    Attendance = e.Attendance != null
                        ? new GetAttendanceInfoResponse
                        {
                            Id = e.Attendance.Id,
                            Student = new GetUserResponse
                            {
                                Id = e.Student.Id ,
                                FullName = e.Student.FullName() ?? "نامشخص",
                                ProfilePicture = e.Student.ProfilePicture!.BuildImageUrl(false)
                            },
                            Status = e.Attendance.Status,
                            RecordedAt = e.Attendance.RecordedAt,
                            Excuse = e.Attendance.Excuse != null
                                ? new GetExcuseInfoResponse
                                {
                                    Id = e.Attendance.Excuse.Id,
                                    Status = e.Attendance.Excuse.Status,
                                    SubmittedAt = e.Attendance.Excuse.SubmittedAt,
                                    Reason = e.Attendance.Excuse.Reason ?? string.Empty,
                                    Attachment = e.Attendance.Excuse.Attachment != null
                                        ? new GetAttachmentInfoResponse
                                        {
                                            Id = e.Attendance.Excuse.Attachment.Id,
                                            FileName = e.Attendance.Excuse.Attachment.FileName ?? "Unknown",
                                            Url = e.Attendance.Excuse.Attachment.Url ?? string.Empty,
                                            ContentType = e.Attendance.Excuse.Attachment.ContentType ?? "application/octet-stream",
                                            UploadedBy = e.Attendance.Excuse.Attachment.UploadedBy,
                                            Uploader = new GetUserResponse
                                            {
                                                Id = e.Attendance.Excuse.Attachment.Uploader.Id ,
                                                FullName = e.Attendance.Excuse.Attachment.Uploader.FullName() ?? "سیستم"
                                            }
                                        }
                                        : null
                                }
                                : null
                        }
                        : null
                })
                .ToList() ?? new List<GetEnrollmentResponse>())

        // Subjects
        .Map(dest => dest.Subjects,
            src => src.Subjects
                .Where(s => s.Subject != null)
                .Select(s => new GetSubjectInfoResponse
                {
                    Id = s.SubjectId,
                    Name = s.Subject.Name ?? "بدون نام"
                })
                .ToList() ?? new List<GetSubjectInfoResponse>());
}

    private static void UserAdaptor()
    {
        TypeAdapterConfig<UpdateUserCommand, User>.NewConfig()
            .Map(dest => dest.ProfilePicture,
                src => src.ProfilePicture != null && src.ProfilePicture.MediaUrl != null
                    ? src.ProfilePicture.MediaUrl
                    : null);

        TypeAdapterConfig<User, GetUserByIdResponse>.NewConfig()
            .Map(dest => dest.Profile,
                src => src.ProfilePicture != null
                    ? src.ProfilePicture!.BuildImageUrl(false)
                    : null)
            .Map(dest => dest.FullName,
                src => src.FullName())
            .Map(dest => dest.ProfileCompress,
                src => src.ProfilePicture != null
                    ? src.ProfilePicture!.BuildImageUrl(true)
                    : null);

        TypeAdapterConfig<User, GetUserResponse>.NewConfig()
            .Map(dest => dest.ProfilePicture,
                src => src.ProfilePicture != null
                    ? src.ProfilePicture!.BuildImageUrl(false)
                    : null);
    }

    private static void OtherAdaptor()
    {
        // Initial university creation
        TypeAdapterConfig<InitialUniversityCommand, UniversityTenantInfo>.NewConfig()
            .Map(dest => dest.Identifier, src => src.Domain);

        // University info response mapping
        TypeAdapterConfig<UniversityTenantInfo, GetUniversityInfoResponse>.NewConfig()
            .Map(dest => dest.Domain, src => src.Identifier)
            .Map(dest => dest.Logo,
                src => src.Logo != null
                    ? src.Logo!.BuildImageUrl(false)
                    : null);

        // User log info
        TypeAdapterConfig<User, GetUserResponse>.NewConfig()
            .Map(dest => dest.CreatedBy,
                src => src.CreatedBy == null
                    ? null
                    : new LogPropertyInfoResponse
                    {
                        Id = src.CreatedBy.Value
                    });

        // Tenant user mapping
        TypeAdapterConfig<UniversityUser, GetUserTenantResponse>.NewConfig()
            .Map(dest => dest.Domain, src => src.UniversityTenantInfo.Identifier)
            .Map(dest => dest.Name,   src => src.UniversityTenantInfo.Name);

        // Ignore IFormFile globally
        TypeAdapterConfig.GlobalSettings.Default.IgnoreMember((member, side) =>
            member.Type == typeof(IFormFile));
    }
}