namespace SmartAttendance.Application.Base.Payment.Commands.Verify;

public record VerifyPaymentCommand(
    string Authority,
    string Status
) : IRequest<string>;