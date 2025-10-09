namespace SmartAttendance.Application.Features.Subjects.Requests.Update;

public class UpdateSubjectRequest  : CreateSubjectRequest
{
    public Guid Id { get; set; }
}