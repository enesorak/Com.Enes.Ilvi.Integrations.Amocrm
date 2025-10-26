using Domain.Pipelines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class PipelinesConfiguration : IEntityTypeConfiguration<Pipeline>
{
    public void Configure(EntityTypeBuilder<Pipeline> builder)
    {
        builder.ToTable("Pipelines");
        builder.HasKey(x => x.Id);
        
        builder.Property(e => e.Id).HasConversion(c => c.Value, value => new PipelineId(value));

        builder.Property(e => e.Raw)
          
            .HasColumnType("NVARCHAR(MAX)");
        
        builder.Property(e => e.Status)
          
            .HasColumnType("NVARCHAR(MAX)");
        
    
        builder.Property(e => e.ComputedHash)
            .HasColumnType("NVARCHAR(MAX)");


        builder
            .HasIndex(e => new { e.Id });

        builder.Property(e => e.SourceUpdatedAtUtc).HasColumnType("datetime");
        builder.Property(e => e.CreatedAtUtc).HasColumnType("datetime");
        builder.Property(e => e.CheckedAtUtc).HasColumnType("datetime");
        builder.Property(e => e.UpdatedAtUtc).HasColumnType("datetime");

    }
}