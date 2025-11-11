public class GetStudentEnrollmentsQuery : IRequest<List<GetEnrollmentResponse>>
{
    public Guid StudentId { get; set; }
}