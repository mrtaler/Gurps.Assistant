using System.Data;
using System.Data.SqlClient;

namespace Gurps.Assistant.Domain.Repository.Interfaces
{
  public interface ISqlHelper
  {
    IDbConnection CreateConnection(string connectionString);

    IDataReader ExecuteReader(IDbConnection connection, string commandText, params SqlParameter[] parameters);

    IDataReader ExecuteReader(IDbConnection connection, string commandText, CommandType commandType, params SqlParameter[] parameters);

    int ExecuteNonQuery(IDbConnection connection, string commandText, params SqlParameter[] parameters);

    TEntity ExecuteScalar<TEntity>(IDbConnection connection, string commandText, params SqlParameter[] parameters);

    int ExecuteNonQuery(IDbConnection connection, string commandText, CommandType commandType, params SqlParameter[] parameters);
  }
}
