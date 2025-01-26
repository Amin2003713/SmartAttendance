using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shifty.Domain.Divisions;

namespace Shifty.Persistence.Configuration.Divisions
{
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
    public class DivisionConfiguration : IEntityTypeConfiguration<Division>
    {
        public void Configure(EntityTypeBuilder<Division> builder)
        {
            builder.HasOne(a => a.Shift).WithMany(a => a.Divisions).HasForeignKey(a => a.ShiftId).OnDelete(DeleteBehavior.Restrict);

            // Configure self-referencing relationship
            builder.HasOne(d => d.Parent) // Each Division has one parent
                   .
                   WithMany(d => d.Children) // Each parent Division can have many children
                   .
                   HasForeignKey(d => d.ParentId) // Foreign key for the parent
                   .
                   OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete
        }
    }
}