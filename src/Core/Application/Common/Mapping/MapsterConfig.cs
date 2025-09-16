using SmartAttendance.Domain.UserAggregate;
using SmartAttendance.Domain.PlanAggregate;
using SmartAttendance.Domain.AttendanceAggregate;
using SmartAttendance.Domain.DocumentAggregate;
using SmartAttendance.Domain.NotificationAggregate;
using SmartAttendance.Application.Features.Users.Responses;
using SmartAttendance.Application.Features.Plans.Responses;
using SmartAttendance.Application.Features.Attendance.Responses;
using SmartAttendance.Application.Features.Documents.Responses;
using SmartAttendance.Application.Features.Notifications.Responses;
using SmartAttendance.Domain.DocumentAggregate;
using SmartAttendance.Domain.NotificationAggregate;

namespace SmartAttendance.Application.Common.Mapping;

// پیکربندی Mapster برای نگاشت دامنه به DTO
public static class MapsterConfig
{
	public static void Register()
	{
		TypeAdapterConfig<UserAggregate, UserProfileDto>.NewConfig()
			.Map(dest => dest.Id, src => src.Id.Value)
			.Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}")
			.Map(dest => dest.Email, src => src.Email.Value)
			.Map(dest => dest.Phone, src => src.Phone.Value)
			.Map(dest => dest.NationalCode, src => src.NationalCode.Value)
			.Map(dest => dest.IsLocked, src => src.IsLocked);

		TypeAdapterConfig<PlanAggregate, PlanDto>.NewConfig()
			.Map(dest => dest.Id, src => src.Id.Value)
			.Map(dest => dest.Title, src => src.Title)
			.Map(dest => dest.Description, src => src.Description)
			.Map(dest => dest.StartsAtUtc, src => src.StartsAtUtc)
			.Map(dest => dest.EndsAtUtc, src => src.EndsAtUtc)
			.Map(dest => dest.Capacity, src => src.Capacity.Capacity)
			.Map(dest => dest.Reserved, src => src.Capacity.Reserved);

		TypeAdapterConfig<AttendanceAggregate, AttendanceDto>.NewConfig()
			.Map(dest => dest.Id, src => src.Id.Value)
			.Map(dest => dest.StudentId, src => src.StudentId.Value)
			.Map(dest => dest.PlanId, src => src.PlanId.Value)
			.Map(dest => dest.Status, src => src.Status.ToString())
			.Map(dest => dest.RecordedAtUtc, src => src.RecordedAtUtc)
			.Map(dest => dest.Points, src => src.Points);

		TypeAdapterConfig<Document, DocumentDto>.NewConfig()
			.Map(dest => dest.Id, src => src.Id.Value)
			.Map(dest => dest.FileName, src => src.FileName)
			.Map(dest => dest.ContentType, src => src.ContentType)
			.Map(dest => dest.SizeBytes, src => src.SizeBytes)
			.Map(dest => dest.UploadedAtUtc, src => src.UploadedAtUtc)
			.Map(dest => dest.AttendanceId, src => src.AttendanceId.HasValue ? src.AttendanceId.Value.Value : (Guid?)null)
			.Map(dest => dest.Status, src => src.Status.ToString());

		TypeAdapterConfig<Notification, NotificationDto>.NewConfig()
			.Map(dest => dest.Id, src => src.Id.Value)
			.Map(dest => dest.RecipientId, src => src.RecipientId.Value)
			.Map(dest => dest.Title, src => src.Title)
			.Map(dest => dest.Message, src => src.Message)
			.Map(dest => dest.Channel, src => src.Channel.ToString())
			.Map(dest => dest.IsSent, src => src.IsSent)
			.Map(dest => dest.IsRead, src => src.IsRead)
			.Map(dest => dest.CreatedAtUtc, src => src.CreatedAtUtc);
	}
}

