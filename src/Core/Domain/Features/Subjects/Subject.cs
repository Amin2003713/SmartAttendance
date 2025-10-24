using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Domain.Features.Majors;

namespace SmartAttendance.Domain.Features.Subjects;

public class Subject  : BaseEntity
{
    public string Name { get; set; }

    public Guid MajorId { get; set; }
    public Major Major { get; set; } = default!;

    public List<SubjectTeacher> Teachers { get; set; }
}