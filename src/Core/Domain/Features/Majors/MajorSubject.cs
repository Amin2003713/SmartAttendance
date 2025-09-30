using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Domain.Features.Majors;

public class MajorSubject : BaseEntity
{
    public Major Major { get; set; }
    public Guid MajorId { get; set; }

    public Guid SubjectId { get; set; }
    public Subject Subject { get; set; }
}