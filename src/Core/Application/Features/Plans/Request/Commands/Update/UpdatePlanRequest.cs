using SmartAttendance.Application.Features.Plans.Request.Commands.Create;

namespace SmartAttendance.Application.Features.Plans.Request.Commands.Update;

public class  UpdatePlanRequest   : CreatePlanRequest  
{
    public Guid Id { get; set; }
}