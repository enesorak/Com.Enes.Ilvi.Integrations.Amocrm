using Domain.Tasks.TaskTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class TaskTypesConfiguration :IEntityTypeConfiguration<TaskType>
{
    public void Configure(EntityTypeBuilder<TaskType> builder)
    {
        builder.ToTable("TaskTypes");
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
            .HasConversion(c => c.Value, value => new TaskTypeId(value));


        builder.Property(e => e.Name)
            .HasMaxLength(100) .HasColumnType("NVARCHAR");;


        builder.Property(e => e.Color)
            .HasMaxLength(20)
            .HasColumnType("NVARCHAR");

        builder.Property(e => e.Code)
            .HasMaxLength(100) .HasColumnType("NVARCHAR");;
        
        builder.Property(e => e.ComputedHash)
            .HasMaxLength(5) .HasColumnType("NVARCHAR");;
          
       
        builder
            .HasIndex(e => new { e.Id});
        
        
        builder.Property(e => e.SourceUpdatedAtUtc).HasColumnType("datetime");
        builder.Property(e => e.CreatedAtUtc).HasColumnType("datetime");
        builder.Property(e => e.CheckedAtUtc).HasColumnType("datetime");
        builder.Property(e => e.UpdatedAtUtc).HasColumnType("datetime");
    }
}