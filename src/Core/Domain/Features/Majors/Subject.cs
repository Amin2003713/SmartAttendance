using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Domain.Features.Subjects;

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