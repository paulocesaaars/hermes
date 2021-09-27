using Deviot.Hermes.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Deviot.Hermes.Infra.SQLite.Bases
{
    [ExcludeFromCodeCoverage]
    public abstract class MappingBase
    {
        protected static void ConfigureBase<TEntity>(EntityTypeBuilder<TEntity> builder, string tableName) where TEntity : Entity
        {
            builder.ToTable(tableName);

            builder.HasKey(t => t.Id);

            builder.HasIndex(t => new { t.Id });
        }
    }
}
