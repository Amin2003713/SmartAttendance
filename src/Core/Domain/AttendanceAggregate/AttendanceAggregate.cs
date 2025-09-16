using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Domain.Common;
using SmartAttendance.Domain.Events;
using SmartAttendance.Domain.Services;
using SmartAttendance.Domain.ValueObjects;

namespace SmartAttendance.Domain.AttendanceAggregate;

// Aggregate Root: حضور و غیاب با روش‌های مختلف ثبت و فرایند معذوریت
public sealed class AttendanceAggregate : AggregateRoot<AttendanceId>
{
    private bool _isRecorded;

    public AttendanceAggregate(AttendanceId id, UserId studentId, PlanId planId)
        : base(id)
    {
        StudentId = studentId;
        PlanId = planId;
    }

    public UserId StudentId { get;  }
    public PlanId PlanId { get;  }

    public AttendanceStatus Status { get; private set; } = AttendanceStatus.Unknown;
    public DateTime? RecordedAtUtc { get; private set; }
    public bool IsExcused { get; private set; }
    public string? ExcusalReason { get; private set; }
    public double? Points { get; private set; }

    public async Task RecordByQrAsync(QRToken token, IAttendanceValidationService validator, CancellationToken ct = default)
    {
        if (_isRecorded) return;

        if (await validator.ValidateQrAsync(token, PlanId, StudentId, ct) is false)
            throw new BusinessRuleViolationException("اعتبارسنجی QR شکست خورد.");

        Status = AttendanceStatus.Present;
        RecordedAtUtc = DateTime.UtcNow;
        _isRecorded = true;

        RaiseDomainEvent(new QRValidatedEvent(Id));
        RaiseDomainEvent(new AttendanceRecordedEvent(Id, StudentId, PlanId, Status));
    }

    public async Task RecordByGpsAsync(
        GPSCoordinate studentLocation,
        GPSCoordinate gateLocation,
        double allowedRadiusMeters,
        IAttendanceValidationService validator,
        CancellationToken ct = default)
    {
        if (_isRecorded) return;

        if (await validator.ValidateGpsAsync(studentLocation, gateLocation, allowedRadiusMeters, ct) is false)
            throw new BusinessRuleViolationException("اعتبارسنجی GPS شکست خورد.");

        Status = AttendanceStatus.Present;
        RecordedAtUtc = DateTime.UtcNow;
        _isRecorded = true;

        RaiseDomainEvent(new GPSValidatedEvent(Id));
        RaiseDomainEvent(new AttendanceRecordedEvent(Id, StudentId, PlanId, Status));
    }

    public async Task RecordManualAsync(UserId approverId, IAttendanceValidationService validator, bool isPresent, CancellationToken ct = default)
    {
        if (_isRecorded) return;

        if (await validator.ValidateManualAsync(approverId, ct) is false)
            throw new ForbiddenOperationException("مجوز ثبت دستی وجود ندارد.");

        Status = isPresent ? AttendanceStatus.Manual : AttendanceStatus.Absent;
        RecordedAtUtc = DateTime.UtcNow;
        _isRecorded = true;

        RaiseDomainEvent(new AttendanceRecordedEvent(Id, StudentId, PlanId, Status));
    }

    public void RecordOffline()
    {
        if (_isRecorded) return;

        Status = AttendanceStatus.OfflineSynced;
        RecordedAtUtc = DateTime.UtcNow;
        _isRecorded = true;

        RaiseDomainEvent(new AttendanceRecordedEvent(Id, StudentId, PlanId, Status));
    }

    public async Task SyncOfflineAsync(IOfflineSyncService syncService, CancellationToken ct = default)
    {
        if (await syncService.SyncAttendanceAsync(Id, ct))
        {
            if (Status == AttendanceStatus.OfflineSynced)
            {
                Status = AttendanceStatus.Present;
                RaiseDomainEvent(new OfflineSyncPerformedEvent(Id));
                RaiseDomainEvent(new AttendanceRecordedEvent(Id, StudentId, PlanId, Status));
            }
        }
    }

    public void ApproveExcusal(string reason)
    {
        reason = reason?.Trim() ?? throw new DomainValidationException("علت معذوریت الزامی است.");
        if (reason.Length < 3) throw new DomainValidationException("علت معذوریت بسیار کوتاه است.");

        IsExcused = true;
        ExcusalReason = reason;

        if (Status is AttendanceStatus.Absent or AttendanceStatus.Late)
        {
            Status = AttendanceStatus.Excused;
        }

        RaiseDomainEvent(new ExcusalApprovedEvent(Id, reason));
    }

    public void CalculatePoints(IPointsCalculationService calculator, TimeSpan? delay = null)
    {
        Points = calculator.CalculatePoints(Status, delay, IsExcused);
        RaiseDomainEvent(new PointsCalculatedEvent(Id, Points.Value));
    }
}