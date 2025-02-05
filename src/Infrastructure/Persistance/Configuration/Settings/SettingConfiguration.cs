using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shifty.Domain.Common.BaseClasses;
using Shifty.Domain.Features.Setting;

namespace Shifty.Persistence.Configuration.Settings;

public class SettingConfiguration    : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.Property(a=>a.Flags)
               .HasConversion<long>();
    }
}