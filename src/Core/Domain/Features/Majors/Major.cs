using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Domain.Features.Majors;

public class Major : BaseEntity
{
    public string Name { get; set; }

    public User? HeadMaster { get; set; }
    public Guid? HeadMasterId { get; set; }

    public ICollection<Subject>? Subjects { get; set; }
}