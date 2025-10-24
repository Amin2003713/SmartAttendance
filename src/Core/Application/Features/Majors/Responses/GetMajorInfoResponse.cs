using SmartAttendance.Domain.Features.Majors;

namespace SmartAttendance.Application.Features.Majors.Responses;

public class GetMajorInfoResponse
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public List<MajorSubjectResponse> Subjects { get; set; }
}