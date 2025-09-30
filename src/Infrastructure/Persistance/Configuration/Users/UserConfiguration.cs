using System.Reactive.Joins;
using SmartAttendance.Domain.Features.Attachments;
using SmartAttendance.Domain.Features.Attendances;
using SmartAttendance.Domain.Features.Excuses;
using SmartAttendance.Domain.Features.Notifications;
using SmartAttendance.Domain.Features.Plans;

namespace SmartAttendance.Persistence.Configuration.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(p => p.UserName).IsRequired().HasMaxLength(100);

        builder.HasMany(p => p.PlansTaught)
            .WithOne(p => p.Teacher)
            .HasForeignKey(p => p.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Users");
    }
}

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.ToTable("Plans");

        builder.Property(p => p.CourseName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder.Property(p => p.Location)
            .HasMaxLength(200);

        builder.Property(p => p.Capacity)
            .IsRequired();

        builder.HasOne(p => p.Teacher)
            .WithMany(u => u.PlansTaught)
            .HasForeignKey(p => p.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class PlanEnrollmentConfiguration : IEntityTypeConfiguration<PlanEnrollment>
{
    public void Configure(EntityTypeBuilder<PlanEnrollment> builder)
    {
        builder.ToTable("PlanEnrollments");

        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(p => p.Plan)
            .WithMany(p => p.Enrollments)
            .HasForeignKey(p => p.PlanId);

        builder.HasOne(p => p.Student)
            .WithMany(u => u.Enrollments)
            .HasForeignKey(p => p.StudentId);
    }
}

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.ToTable("Attendance");

        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(p => p.Plan)
            .WithMany(p => p.Attendances)
            .HasForeignKey(p => p.PlanId);

        builder.HasOne(p => p.Student)
            .WithMany(u => u.Attendances)
            .HasForeignKey(p => p.StudentId);

        builder.HasOne(p => p.Excuse)
            .WithMany()
            .HasForeignKey(p => p.ExcuseId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class ExcuseConfiguration : IEntityTypeConfiguration<Excuse>
{
    public void Configure(EntityTypeBuilder<Excuse> builder)
    {
        builder.ToTable("Excuses");

        builder.Property(p => p.Reason)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(p => p.Student)
            .WithMany(u => u.Excuses)
            .HasForeignKey(p => p.StudentId);

        builder.HasOne(p => p.Plan)
            .WithMany()
            .HasForeignKey(p => p.PlanId);

        builder.HasOne(p => p.Attachment)
            .WithMany()
            .HasForeignKey(p => p.AttachmentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.ToTable("Attachments");

        builder.Property(p => p.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.ContentType)
            .HasMaxLength(100);

        builder.HasOne(p => p.Uploader)
            .WithMany()
            .HasForeignKey(p => p.UploadedBy);
    }
}

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne(p => p.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(p => p.UserId);
    }
}