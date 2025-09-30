namespace SmartAttendance.Application.Features.Plans.Responses;

public record GetEnrollmentResponse(
    string StudentName ,
    string PersonalNumber ,
    Guid Id
);