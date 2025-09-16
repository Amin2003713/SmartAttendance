namespace SmartAttendance.Application.Features.Plans.Commands;

// Command: ثبت‌نام دانشجو در طرح
public sealed record EnrollStudentCommand(
    Guid PlanId,
    Guid StudentId
) : IRequest;

// Handler: ثبت‌نام دانشجو