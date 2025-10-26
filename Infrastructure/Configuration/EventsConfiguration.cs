using Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class EventsConfiguration :IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(e => e.Id).HasMaxLength(100).HasColumnType("NVARCHAR").HasConversion(c => c.Value, value => new EventId(value));
 
        builder.Property(e => e.EntityType)
            .HasMaxLength(50)
            .HasColumnType("NVARCHAR");
        
        builder.Property(e => e.EventType)
            .HasMaxLength(50)
            .HasColumnType("NVARCHAR");
  
        builder.Property(e => e.Raw) 
         
                 .HasColumnType("NVARCHAR(MAX)");
        
        builder.Property(e => e.ComputedHash)
         
            .HasColumnType("NVARCHAR(MAX)");

        builder.Property(e => e.EventAtUtc).HasColumnType("datetime");
        
        builder.Property(e => e.SourceUpdatedAtUtc).HasColumnType("datetime");
        builder.Property(e => e.CreatedAtUtc).HasColumnType("datetime");
        builder.Property(e => e.CheckedAtUtc).HasColumnType("datetime");
        builder.Property(e => e.UpdatedAtUtc).HasColumnType("datetime");



    }
}