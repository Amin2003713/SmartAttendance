namespace Shifty.Application.Base.ZarinPal.Request;

public record ZarinPalVerifyRequest(
    long Amount,
    string Status,
    string Authority
);