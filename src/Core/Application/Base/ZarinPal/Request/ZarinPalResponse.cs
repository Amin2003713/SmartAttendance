namespace SmartAttendance.Application.Base.ZarinPal.Request;

public class ZarinPalResponse : Riviera.ZarinPal.V4.Models.Payment
{
    public bool Success => PaymentUri != null && Code == 100;
}