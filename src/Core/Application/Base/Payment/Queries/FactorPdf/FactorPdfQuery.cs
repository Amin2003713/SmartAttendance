namespace Shifty.Application.Base.Payment.Queries.FactorPdf;

public class FactorPdfQuery : IRequest<string>
{
    private FactorPdfQuery(Guid paymentId)
    {
        PaymentId = paymentId;
    }

    public Guid PaymentId { get; }

    public static FactorPdfQuery Create(Guid paymentId)
    {
        return new FactorPdfQuery(paymentId);
    }
}