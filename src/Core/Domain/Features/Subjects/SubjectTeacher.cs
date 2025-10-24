using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Domain.Features.Subjects;

public class SubjectTeacher  : BaseEntity
{
    public Guid TeacherId { get; set; }
    public User Teacher { get; set; } = default!;


    public Guid SubjectId { get; set; }
    public Subject Subject { get; set; } = default!;
}