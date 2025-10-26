using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;



internal static class ConnectionStringFactory 
{
    public static string GetConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")!;
        var db = configuration.GetValue<string>("Options:Db");
        return connectionString.Replace("{Database}", db);
    }
}

 