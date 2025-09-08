using SmartAttendance.Domain.Messages;

namespace SmartAttendance.Persistence.Configuration.Messages;

internal class MessageConfig : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasMany(m => m.Comments).WithOne(c => c.Message).HasForeignKey(c => c.MessageId);

        builder.HasMany(m => m.UserVisitedMessages).WithOne(uvm => uvm.Message).HasForeignKey(uvm => uvm.MessageId);

        builder.HasMany(m => m.UserLikedMessages).WithOne(ulm => ulm.Message).HasForeignKey(ulm => ulm.MessageId);

        builder.HasMany(m => m.UserTargetMessages).WithOne(utm => utm.Message).HasForeignKey(utm => utm.MessageId);
    }
}