using Shifty.Domain.Setting;

namespace Shifty.Persistence.Configuration.Settings;

public class SettingConfiguration : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.Property(a => a.Flags).HasConversion<long>();
    }
}