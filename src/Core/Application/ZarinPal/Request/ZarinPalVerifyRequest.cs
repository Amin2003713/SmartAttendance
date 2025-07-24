namespace Shifty.Application.ZarinPal.Request;

public record ZarinPalVerifyRequest(
    long Amount,
    string Status,
    string Authority
);