using System.Data;
using System.Transactions;
using Dapper;
using MySql.Data.MySqlClient;

namespace serilog_seq_demo.Common.Maria;


/// <summary>
/// Maria Dapper Helper
/// </summary>
public static class MariaHelper
{
    /// <summary>
    /// Executes a SQL statement against the connection and returns the number of rows affected.
    /// </summary>
    /// <param name="connectionString">A valid connection string for a MySqlConnection</param>
    /// <param name="commandType">The CommandType(stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of MySqlParameter used to execute the command</param>
    /// <returns>Number of rows affected</returns>
    public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
    {
        if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));
        if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException(nameof(commandText));

        int affected;

        using (MySqlConnection connection = new(connectionString))
        {
            connection.Open();
            using MySqlCommand command = new(commandText, connection);
            command.CommandType = commandType;

            if (commandParameters != null)
            {
                foreach (MySqlParameter param in commandParameters)
                {
                    if (param == null) continue;

                    if ((param.Direction == ParameterDirection.Input || param.Direction == ParameterDirection.InputOutput)
                      && param.Value == null)
                    {
                        param.Value = DBNull.Value;
                    }

                    command.Parameters.Add(param);
                }
            }

            affected = command.ExecuteNonQuery();
        }

        return affected;
    }

    /// <summary>
    /// Executes a query, returning the data typed as per T
    /// </summary>
    /// <typeparam name="T">data type</typeparam>
    /// <param name="connectionString">A valid connection string for a MySqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="parameters"> An array of MySqlParameter used to execute the command</param>
    /// <returns>IEnumerable T</returns>
    public static IEnumerable<T> QueryCollection<T>(string connectionString, CommandType commandType, string commandText, params MySqlParameter[] parameters)
    {
        return query<T>(connectionString, commandType, commandText, parameters);
    }

    /// <summary>
    /// Executes a query, returning the data typed as per T
    /// </summary>
    /// <typeparam name="T">data type</typeparam>
    /// <param name="connectionString">A valid connection string for a MySqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="parameters"> An array of MySqlParameter used to execute the command</param>
    /// <returns>T</returns>
    public static T? Query<T>(string connectionString, CommandType commandType, string commandText, params MySqlParameter[] parameters)
    {
        return query<T>(connectionString, commandType, commandText, parameters).FirstOrDefault();
    }

    /// <summary>
    /// Executes a query, returning the data typed as per T
    /// </summary>
    /// <typeparam name="T">data type</typeparam>
    /// <param name="connectionString">A valid connection string for a MySqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of MySqlParameter used to execute the command</param>
    /// <returns>IEnumerable T</returns>
    private static IEnumerable<T> query<T>(string connectionString, CommandType commandType, string commandText, MySqlParameter[] commandParameters)
    {
        if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));
        if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException(nameof(commandText));
        IEnumerable<T> result;

        TransactionOptions transactionOptions = new()
        {
            IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
        };

        using (TransactionScope scope = new(TransactionScopeOption.RequiresNew, transactionOptions))
        {
            using MySqlConnection connection = new(connectionString);
            DynamicParameters? parameters = ParseParameters(commandParameters);
            result = connection.Query<T>(commandText, parameters, commandType: commandType);
            SetOutputParameterValue(commandParameters, parameters);
        }

        return result;
    }

    /// <summary>
    /// Parse SqlParameter To DynamicParameters
    /// </summary>
    /// <param name="commandParameters">SqlParameter</param>
    /// <returns>DynamicParameters</returns>
    private static DynamicParameters? ParseParameters(MySqlParameter[] commandParameters)
    {
        if (commandParameters.Length <= 0) return null;
        
        var result = new DynamicParameters();
        foreach (var p in commandParameters)
        {
            if (p.Direction is ParameterDirection.Input or ParameterDirection.InputOutput && p.Value == null)
            {
                p.Value = DBNull.Value;
            }

            result.Add(p.ParameterName, p.Value, p.DbType, p.Direction);
        }

        return result;
    }

    /// <summary>
    /// Set Output Parameter Value
    /// </summary>
    /// <param name="parameters">An array of MySqlParameter used to execute the command</param>
    /// <param name="dynamicParameters">A bag of parameters that can be passed to the Dapper Query and Execute methods</param>
    private static void SetOutputParameterValue(MySqlParameter[] parameters, DynamicParameters? dynamicParameters)
    {
        if (parameters != null && dynamicParameters != null)
        {
            foreach (MySqlParameter p in parameters)
            {
                if (p.Direction == ParameterDirection.Input) continue;

                p.Value = dynamicParameters.Get<object>(p.ParameterName);
            }
        }
    }
}