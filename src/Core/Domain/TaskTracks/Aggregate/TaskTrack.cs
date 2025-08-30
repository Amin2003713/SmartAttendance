using Mapster;
using Shifty.Common.General.Enums;
using Shifty.Domain.TaskTracks.Events.TaskTrackers;
using Shifty.Domain.TaskTracks.Events.TaskTrackReports;
using Shifty.Domain.TaskTracks.SnapShots;
using TaskTracker.Domain.TaskTracks;

namespace Shifty.Domain.TaskTracks.Aggregate;

public class TaskTrack : AggregateRoot<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid? WorkPackageId { get; set; }
    public PriorityType PriorityType { get; set; }
    public List<Guid> AssigneeId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    [JsonIgnore] public DateTime Reported { get; set; }
    public TasksStatus Status { get; set; }

    // Report info (optional section)
    public List<TaskTrackReport> Reports { get; set; } = new();


    #region Factory

    public static TaskTrack New(TaskTrackCreatedEvent @event)
    {
        var task = new TaskTrack();
        task.Create(@event);
        return task;
    }

    #endregion

    #region Behavioral Methods

    public void Create(TaskTrackCreatedEvent @event)
    {
        RaiseEvent(@event);
    }

    public void Update(TaskTrackUpdatedEvent @event)
    {
        RaiseEvent(@event);
    }

    public void Delete(TaskTrackDeletedEvent @event)
    {
        RaiseEvent(@event);
    }

    public void AddReport(TaskTrackReportCreatedEvent @event)
    {
        RaiseEvent(@event);
    }

    public void UpdateReport(TaskTrackReportUpdatedEvent @event)
    {
        RaiseEvent(@event);
    }

    public void DeleteReport(TaskTrackReportDeletedEvent @event)
    {
        RaiseEvent(@event);
    }

    #endregion

    #region Apply Methods

    public void Apply(TaskTrackCreatedEvent @event)
    {
        AggregateId = @event.AggregateId;
        AssigneeId = @event.AssigneeId;
        CreatedBy = @event.CreatedBy;
        Description = @event.Description;
        PriorityType = @event.PriorityType;
        Title = @event.Title;
        Status = @event.Status;
        EndDate = @event.EndDate;
        StartDate = @event.StartDate;
        WorkPackageId = @event.WorkPackageId;
        Reported = @event.Reported;
    }

    public void Apply(TaskTrackUpdatedEvent @event)
    {
        AggregateId = @event.AggregateId;
        AssigneeId = @event.AssigneeId;
        CreatedBy = @event.CreatedBy;
        Description = @event.Description;
        PriorityType = @event.PriorityType;
        Title = @event.Title;
        Status = @event.Status;
        EndDate = @event.EndDate;
        StartDate = @event.StartDate;
        WorkPackageId = @event.WorkPackageId;
    }

    public void Apply(TaskTrackDeletedEvent @event)
    {
        AggregateId = @event.AggregateId;
        Deleted = true;
    }

    public void Apply(TaskTrackReportCreatedEvent @event)
    {
        Reports.Add(new TaskTrackReport
        {
            ReportId = @event.ReportId,
            ReportDetail = @event.ReportDetail,
            ReportCreatedBy = @event.ReportCreatedBy,
            StartReportDate = @event.StartReportDate,
            EndReportDate = @event.EndReportDate,
            WorkTime = @event.WorkTime
        });
    }

    public void Apply(TaskTrackReportUpdatedEvent @event)
    {
        var report = Reports.FirstOrDefault(r => r.ReportId == @event.ReportId);
        if (report is null) return;

        report.ReportDetail = @event.ReportDetail;
        report.ReportCreatedBy = @event.ReportCreatedBy;
        report.StartReportDate = @event.StartReportDate;
        report.EndReportDate = @event.EndReportDate;
        report.WorkTime = @event.WorkTime;
    }

    public void Apply(TaskTrackReportDeletedEvent @event)
    {
        var report = Reports.FirstOrDefault(r => r.ReportId == @event.ReportId);
        if (report is not null)
            Reports.Remove(report);
    }

    #endregion

    #region Snapshot

    protected override void RestoreFromSnapshot(ISnapshot<Guid> snapshot)
    {
        if (snapshot is not TaskTrackSnapShot s)
            throw new InvalidCastException("Invalid snapshot type");

        AggregateId = s.AggregateId;
        Version = s.Version;
        AssigneeId = s.AssigneeId;
        CreatedBy = s.CreatedBy;
        Title = s.Title;
        Description = s.Description;
        PriorityType = s.PriorityType;
        Status = s.Status;
        StartDate = s.StartDate;
        EndDate = s.EndDate;
        WorkPackageId = s.WorkPackageId;
        Reported = s.Reported;
        Reports = s.Reports;
        Deleted = s.Deleted;
    }

    public override ISnapshot<Guid> GetSnapshot()
    {
        return this.Adapt<TaskTrackSnapShot>();
    }

    #endregion
}