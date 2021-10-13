using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Infra.SQLite.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Deviot.Hermes.Infra.SQLite.Mapping
{
    [ExcludeFromCodeCoverage]
    public class DeviceMapping : MappingBase, IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            ConfigureBase<Device>(builder, $"Device");

            builder.Property(o => o.Name)
                .IsRequired();

            builder.Property(o => o.TypeId)
                .IsRequired();

            builder.Property(o => o.Configuration)
                .IsRequired();

            builder.HasIndex(t => new { t.Name });
        }
    }
}
