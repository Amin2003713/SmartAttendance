using SmartAttendance.Application.Features.Subjects.Requests;
using SmartAttendance.Domain.Features.Majors;

namespace SmartAttendance.Application.Features.Majors.Requests.Create;

public class CreateMajorRequest
{
    public string Name { get; set; }
    public Guid? HeadMasterId { get; set; }
}