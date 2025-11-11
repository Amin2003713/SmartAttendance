public class DeletePlanEnrollmentCommand : IRequest
{
    public Guid PlanId { get; set; }
    public Guid StudentId { get; set; }
}