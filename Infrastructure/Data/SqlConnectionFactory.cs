using System.Data;
using Application.Abstractions.Data;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Data;

internal sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        var connection = new SqlConnection(connectionString);
        connection.Open();

        return connection;
    }
}