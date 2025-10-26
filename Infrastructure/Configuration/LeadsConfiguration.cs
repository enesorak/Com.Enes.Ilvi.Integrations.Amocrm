using Domain.Leads;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class LeadsConfiguration : IEntityTypeConfiguration<Lead>
{
    public void Configure(EntityTypeBuilder<Lead> builder)
    {
        builder.ToTable("Leads");
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id).HasConversion(c => c.Value, value => new LeadId(value));

        builder.Property(e => e.Raw) 
            .HasColumnType("NVARCHAR(MAX)");

        builder.Property(e => e.Company)
            .HasColumnType("NVARCHAR(MAX)"); 

        builder.Property(e => e.Tag) 
            .HasColumnType("NVARCHAR(MAX)"); 

        builder.Property(e => e.ComputedHash) 
            .HasColumnType("NVARCHAR(200)");
        
        
        builder
            .HasIndex(e => new { e.Id});

        builder.Property(e => e.SourceUpdatedAtUtc).HasColumnType("datetime");
        builder.Property(e => e.CreatedAtUtc).HasColumnType("datetime");
        builder.Property(e => e.CheckedAtUtc).HasColumnType("datetime");
        builder.Property(e => e.UpdatedAtUtc).HasColumnType("datetime");
    }
}