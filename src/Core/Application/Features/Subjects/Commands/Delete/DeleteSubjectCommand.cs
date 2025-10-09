namespace SmartAttendance.Application.Features.Subjects.Commands.Delete;

public record DeleteSubjectCommand(
    Guid Id
) : IRequest;