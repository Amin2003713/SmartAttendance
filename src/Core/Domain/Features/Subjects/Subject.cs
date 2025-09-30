using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Domain.Features.Subjects;

public class Subject  : BaseEntity
{
    public string Name { get; set; }

    public Guid TeacherId { get; set; }
    public User Teacher { get; set; } = default!;
}