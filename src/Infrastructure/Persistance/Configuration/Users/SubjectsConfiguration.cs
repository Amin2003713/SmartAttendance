using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Configuration.Users;

public class SubjectsConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasQueryFilter(a => a.IsActive);
    }
}