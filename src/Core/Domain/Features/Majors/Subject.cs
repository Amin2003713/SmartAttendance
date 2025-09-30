using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Domain.Features.Majors;

public class Major : BaseEntity
{
    public string Name { get; set; }

    public List<MajorSubject> Subjects { get; set; }
}

public class MajorSubject : BaseEntity
{
    public Major Major { get; set; }
    public Guid MajorId { get; set; }

    public Guid SubjectId { get; set; }
    public Subject Subject { get; set; }
}