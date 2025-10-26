namespace Presentation.Endpoints;

public static class ConfigurationEndpoints
{
    public static void MapConfigurationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        
        endpoints.MapPost("api/configuration",
            (ILogger<string> logger) =>
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                logger.LogInformation($"Environment: {environment}");
                return Results.Ok($"Current environment: {environment}");
            }).WithName("Configuration");
        
        endpoints.MapPost("api/configuration/get-company",
            (IConfiguration configuration) =>
            {
                var company = configuration.GetValue<string>("Company");
                return Results.Ok($"Current company: {company}");
            }).WithName("Current Company");
        
        endpoints.MapPost("api/configuration/get-message",
            (IConfiguration configuration) =>
            {
                var company = configuration.GetValue<string>("Control");
                return Results.Ok($"Message {company}");
            }).WithName("Message Control");
    }
}