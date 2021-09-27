using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Infra.SQLite.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Deviot.Hermes.Infra.SQLite.Mapping
{
    [ExcludeFromCodeCoverage]
    public class UserMapping : MappingBase, IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            ConfigureBase<User>(builder, $"User");

            builder.Property(o => o.FullName)
                .HasColumnType("varchar(150)")
                .IsRequired();

            builder.Property(o => o.Enabled)
                .IsRequired();

            builder.Property(o => o.UserName)
                .HasColumnType("varchar(20)")
                .IsRequired();

            builder.Property(o => o.Password)
                .HasColumnType("varchar(300)")
                .IsRequired();

            builder.HasIndex(t => new { t.UserName, t.Password, t.Enabled });

            builder.HasIndex(t => new { t.UserName }).IsUnique();

            builder.HasIndex(t => new { t.FullName });
        }
    }
}
