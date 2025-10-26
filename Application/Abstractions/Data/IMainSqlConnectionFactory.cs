using System.Data;

namespace Application.Abstractions.Data;

public interface IMainSqlConnectionFactory
{
    IDbConnection CreateConnection();
}