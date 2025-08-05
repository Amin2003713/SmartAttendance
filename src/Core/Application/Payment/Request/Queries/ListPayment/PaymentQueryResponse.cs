using Shifty.Common.General.Enums.Payment;

namespace Shifty.Application.Payment.Request.Queries.ListPayment;

public class PaymentQueryResponse
{
    public Guid Id { get; set; }
    public decimal Cost { get; set; }
    public decimal GrantedStorageMb { get; set; }
    public int? Duration { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? RefId { get; set; }
    public bool Status { get; set; }
    public PaymentType PaymentType { get; set; }
    public List<string> AcvtiveServices { get; set; }
}