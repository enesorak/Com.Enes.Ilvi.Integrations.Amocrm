namespace Presentation.Endpoints;

public static class EnvironmentEndpoint
{
    public static void MapEnvironmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/environment",
            ( ) =>
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                return Results.Ok($"Current environment: {environment}");
                
            }).WithName("Environment");
    }
}