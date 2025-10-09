using SmartAttendance.Application.Features.Majors.Requests.Create;

namespace SmartAttendance.Application.Features.Majors.Commands.Update;

public class UpdateMajorCommand    : UpdateMajorRequest  ,
    IRequest
{
    public Guid Id { get; set; }
}