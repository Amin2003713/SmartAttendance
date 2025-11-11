using SmartAttendance.Application.Interfaces.Attendances;
using SmartAttendance.Common.Exceptions;

public class UpdateAttendanceStatusCommandHandler(
    IAttendanceCommandRepository commandRepository ,
    IAttendanceQueryRepository Repository 

) : IRequestHandler<UpdateAttendanceStatusCommand>
{
    public async Task Handle(UpdateAttendanceStatusCommand request, CancellationToken cancellationToken)
    {
        var attendance = await Repository.GetByIdAsync(cancellationToken, request.AttendanceId);
        if (attendance == null)
            throw SmartAttendanceException.NotFound("Attendance not found");

        attendance.Status = request.Status;
        attendance.ExcuseId = request.ExcuseId;
        await commandRepository.UpdateAsync(attendance, cancellationToken);
    }
}