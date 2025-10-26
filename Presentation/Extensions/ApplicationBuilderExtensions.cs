using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Extensions;

public static class ApplicationBuilderExtensions
{
    
    
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        using var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); 
        context.Database.Migrate();
        context.Database.EnsureCreated();
    }
}