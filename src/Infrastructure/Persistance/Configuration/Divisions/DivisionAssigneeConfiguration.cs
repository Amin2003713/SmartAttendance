using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shifty.Domain.Features.Divisions;

namespace Shifty.Persistence.Configuration.Divisions;

public class DivisionAssigneeConfiguration : IEntityTypeConfiguration<DivisionAssignee>
{
    public void Configure(EntityTypeBuilder<DivisionAssignee> builder)
    {
        builder.HasOne(da => da.Division).WithMany(d => d.Assignees).HasForeignKey(da => da.DivisionId).OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(da => da.User)
               .WithMany(a=>a.AssignedDivisions) // No back-reference needed
               .HasForeignKey(da => da.UserId).
               OnDelete(DeleteBehavior.Cascade);
    }
}