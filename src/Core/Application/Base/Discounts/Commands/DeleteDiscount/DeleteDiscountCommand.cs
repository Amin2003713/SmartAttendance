namespace SmartAttendance.Application.Base.Discounts.Commands.DeleteDiscount;

public class DeleteDiscountCommand(
    Guid id
) : IRequest
{
    public Guid Id { get; set; } = id;
}