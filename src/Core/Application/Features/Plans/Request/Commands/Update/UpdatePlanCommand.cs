using SmartAttendance.Application.Features.Plans.Request.Commands.Create;

namespace SmartAttendance.Application.Features.Plans.Commands.Update;

public class  UpdatePlanRequest   : CreatePlanRequest  ,
    IRequest
{
    public Guid Id { get; set; }
}