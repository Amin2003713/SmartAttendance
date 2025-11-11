public class GetPlanEnrollmentsQuery : IRequest<List<GetEnrollmentResponse>>
{
    public Guid PlanId { get; set; }
}