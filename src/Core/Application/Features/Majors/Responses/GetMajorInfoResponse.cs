using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Application.Features.Majors.Responses;

public class GetMajorInfoResponse
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public List<MajorSubject> Subjects { get; set; }
}

record MajorSubjectResponse(
    Guid Id ,
    string Name
)              ;