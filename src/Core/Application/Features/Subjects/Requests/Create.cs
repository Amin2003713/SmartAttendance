namespace SmartAttendance.Application.Features.Subjects.Requests;

public class CreateSubjectRequest
{
    public List<Guid> TeacherIds { get ; set ; }
    public Guid MajorId { get ; set ; }
    public string Name { get ; set ; }
}