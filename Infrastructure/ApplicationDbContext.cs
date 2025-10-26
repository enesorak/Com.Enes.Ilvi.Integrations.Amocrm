using Application.Abstractions.Clock;
using Application.Exceptions;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    private const string ConnectionString =
        "Server=167.235.255.246;Database=IlviCrm;User Id=majorette;Password=TmzUheE4bxyqvn; Trusted_Connection=false;TrustServerCertificate=True;MultipleActiveResultSets=true";

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IDateTimeProvider dateTimeProvider)
        : base(options)
    {
    }


    public ApplicationDbContext()
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", ex);
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseSqlServer(ConnectionString, options => { options.EnableRetryOnFailure(); });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_100_CI_AS_SC_UTF8");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}