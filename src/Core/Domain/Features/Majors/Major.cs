using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Domain.Features.Majors;

public class Major : BaseEntity
{
    public string Name { get; set; }

    public User? HeadMaster { get; set; }
    public Guid? HeadMasterId { get; set; }

    public ICollection<MajorSubject>? Subjects { get; set; }
}