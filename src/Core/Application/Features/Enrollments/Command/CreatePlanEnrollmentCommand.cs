public class CreatePlanEnrollmentCommand : IRequest<Guid>
{
    public Guid PlanId { get; set; }
    public Guid StudentId { get; set; }
}