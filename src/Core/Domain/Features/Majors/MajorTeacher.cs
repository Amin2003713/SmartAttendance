using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Domain.Features.Majors;

public class MajorTeacher  : BaseEntity
{
    public Guid TeacherId { get; set; }
    public User Teacher { get; set; } = default!;


    public Guid MajorId { get; set; }
    public User Major { get; set; } = default!;
}