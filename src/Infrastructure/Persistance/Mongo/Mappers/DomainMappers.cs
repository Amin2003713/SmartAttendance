using SmartAttendance.Domain.AttendanceAggregate;
using SmartAttendance.Domain.Common;
using SmartAttendance.Domain.DocumentAggregate;
using SmartAttendance.Domain.NotificationAggregate;
using SmartAttendance.Domain.PlanAggregate;
using SmartAttendance.Domain.UserAggregate;
using SmartAttendance.Domain.ValueObjects;
using SmartAttendance.Persistence.Mongo.Documents;

namespace SmartAttendance.Persistence.Mongo.Mappers;

// نگاشت دامنه ↔ سندهای MongoDB
public static class DomainMappers
{
    public static UserDocument ToDocument(this UserAggregate user)
    {
        return new UserDocument
        {
            Id = user.Id.Value,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email.Value,
            Phone = user.Phone.Value,
            NationalCode = user.NationalCode.Value,
            FailedLoginCount = user.FailedLoginCount,
            IsLocked = user.IsLocked,
            LockedAtUtc = user.LockedAtUtc,
            RoleIds = user.Roles.Select(r => r.Value).ToList()
        };
    }

    public static UserAggregate ToDomain(this UserDocument doc)
    {
        var agg = new UserAggregate(new UserId(doc.Id),
            doc.FirstName,
            doc.LastName,
            new EmailAddress(doc.Email),
            new PhoneNumber(doc.Phone),
            new NationalCode(doc.NationalCode));

        foreach (var rid in doc.RoleIds)
        {
            agg.AssignRole(new RoleId(rid));
        }

        return agg;
    }

    public static PlanDocument ToDocument(this PlanAggregate plan)
    {
        return new PlanDocument
        {
            Id = plan.Id.Value,
            Title = plan.Title,
            Description = plan.Description,
            StartsAtUtc = plan.StartsAtUtc,
            EndsAtUtc = plan.EndsAtUtc,
            Capacity = plan.Capacity.Capacity,
            Reserved = plan.Capacity.Reserved,
            Students = plan.Students.Select(x => x.Value).ToList(),
            WaitingList = plan.WaitingList.Select(x => x.Value).ToList()
        };
    }

    public static PlanAggregate ToDomain(this PlanDocument doc)
    {
        var agg = new PlanAggregate(new PlanId(doc.Id), doc.Title, doc.Description, doc.StartsAtUtc, doc.EndsAtUtc, new PlanCapacity(doc.Capacity, doc.Reserved));

        foreach (var s in doc.Students)
        {
            agg.RegisterStudent(new UserId(s));
        }

        return agg;
    }

    public static AttendanceDocument ToDocument(this AttendanceAggregate att)
    {
        return new AttendanceDocument
        {
            Id = att.Id.Value,
            StudentId = att.StudentId.Value,
            PlanId = att.PlanId.Value,
            Status = att.Status.ToString(),
            RecordedAtUtc = att.RecordedAtUtc,
            IsExcused = att.IsExcused,
            ExcusalReason = att.ExcusalReason,
            Points = att.Points
        };
    }

    public static AttendanceAggregate ToDomain(this AttendanceDocument doc)
    {
        return new AttendanceAggregate(new AttendanceId(doc.Id), new UserId(doc.StudentId), new PlanId(doc.PlanId));
    }

    public static DocumentDocument ToDocument(this Document d)
    {
        return new DocumentDocument
        {
            Id = d.Id.Value,
            FileName = d.FileName,
            ContentType = d.ContentType,
            SizeBytes = d.SizeBytes,
            UploadedAtUtc = d.UploadedAtUtc,
            AttendanceId = d.AttendanceId?.Value,
            Status = (int)d.Status
        };
    }

    public static Document ToDomain(this DocumentDocument doc)
    {
        return new Document(new DocumentId(doc.Id),
            doc.FileName,
            doc.ContentType,
            doc.SizeBytes,
            doc.UploadedAtUtc,
            doc.AttendanceId.HasValue ? new AttendanceId(doc.AttendanceId.Value) : null);
    }

    public static NotificationDocument ToDocument(this Notification n)
    {
        return new NotificationDocument
        {
            Id = n.Id.Value,
            RecipientId = n.RecipientId.Value,
            Title = n.Title,
            Message = n.Message,
            Channel = n.Channel.ToString(),
            IsSent = n.IsSent,
            CreatedAtUtc = n.CreatedAtUtc
        };
    }

    public static Notification ToDomain(this NotificationDocument doc)
    {
        var channel = Enum.TryParse<NotificationChannel>(doc.Channel, out var ch) ? ch : NotificationChannel.Unknown;
        var n       = new Notification(new NotificationId(doc.Id), new UserId(doc.RecipientId), doc.Title, doc.Message, channel);
        if (doc.IsSent) n.MarkAsSent();

        if (doc.Channel?.Equals("InApp", StringComparison.OrdinalIgnoreCase) == true && doc.IsSent && doc.CreatedAtUtc <= DateTime.UtcNow) { }

        return n;
    }
}