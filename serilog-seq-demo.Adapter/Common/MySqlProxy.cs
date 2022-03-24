using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using serilog_seq_demo.Common.Maria;

namespace serilog_seq_demo.Adapter.Common;

internal static class MySqlProxy
{
    /// <summary>
    /// Executes a SQL statement against the connection and returns the number of rows affected.
    /// </summary>
    /// <param name="connectionString">A valid connection string for a MySqlConnection</param>
    /// <param name="commandType">The CommandType(stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of MySqlParameter used to execute the command</param>
    /// <returns>Number of rows affected</returns>
    internal static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        int result = MariaHelper.ExecuteNonQuery(connectionString, commandType, commandText, commandParameters);
        stopwatch.Stop();
        PerformanceLogger.Write(Database.MySql, commandText, stopwatch.Elapsed, GetParams(commandParameters));
        return result;
    }

    /// <summary>
    /// Executes a SQL statement against the connection and returns the number of rows affected.
    /// </summary>
    /// <param name="connectionString">A valid connection string for a MySqlConnection</param>
    /// <param name="commandType">The CommandType(stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="isLog">Whether to record</param>
    /// <param name="commandParameters">An array of MySqlParameter used to execute the command</param>
    /// <returns>Number of rows affected</returns>
    internal static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, bool isLog = true, params MySqlParameter[] commandParameters)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        int result = MariaHelper.ExecuteNonQuery(connectionString, commandType, commandText, commandParameters);
        stopwatch.Stop();
        if (isLog) PerformanceLogger.Write(Database.MySql, commandText, stopwatch.Elapsed, GetParams(commandParameters));
        return result;
    }

    /// <summary>
    /// Executes a query, returning the data typed as per T
    /// </summary>
    /// <typeparam name="T">data type</typeparam>
    /// <param name="connectionString">A valid connection string for a MySqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters"> An array of MySqlParameter used to execute the command</param>
    /// <returns>T</returns>
    internal static T? Query<T>(string connectionString, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        T? result = MariaHelper.Query<T>(connectionString, commandType, commandText, commandParameters);
        stopwatch.Stop();
        PerformanceLogger.Write(Database.MySql, commandText, stopwatch.Elapsed, GetParams(commandParameters));

        return result;
    }

    /// <summary>
    /// Executes a query, returning the data typed as per T
    /// </summary>
    /// <typeparam name="T">data type</typeparam>
    /// <param name="connectionString">A valid connection string for a MySqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="isLog">Whether to record</param>
    /// <param name="commandParameters"> An array of MySqlParameter used to execute the command</param>
    /// <returns>T</returns>
    internal static T? Query<T>(string connectionString, CommandType commandType, string commandText, bool isLog = true, params MySqlParameter[] commandParameters)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        T? result = MariaHelper.Query<T>(connectionString, commandType, commandText, commandParameters);
        stopwatch.Stop();
        if (isLog) PerformanceLogger.Write(Database.MySql, commandText, stopwatch.Elapsed, GetParams(commandParameters));

        return result;
    }

    /// <summary>
    /// Executes a query, returning the data typed as per T
    /// </summary>
    /// <typeparam name="T">data type</typeparam>
    /// <param name="connectionString">A valid connection string for a MySqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters"> An array of MySqlParameter used to execute the command</param>
    /// <returns>IEnumerable T</returns>
    internal static IEnumerable<T> QueryCollection<T>(string connectionString, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        IEnumerable<T> result = MariaHelper.QueryCollection<T>(connectionString, commandType, commandText, commandParameters);
        stopwatch.Stop();
        PerformanceLogger.Write(Database.MySql, commandText, stopwatch.Elapsed, GetParams(commandParameters));

        return result;
    }

    /// <summary>
    /// 取得執行參數字典表物件
    /// </summary>
    /// <param name="commandParameters">執行參數</param>
    /// <returns>執行參數字典表物件</returns>
    private static Dictionary<string, string?>? GetParams(params MySqlParameter[] commandParameters)
    {
        Dictionary<string, string?>? result = null;
        if (commandParameters == null) return result;

        result = new Dictionary<string, string?>();
        foreach (var param in commandParameters)
        {
            if (param == null) continue;

            result.Add(param.ParameterName, param.Value?.ToString());
        }

        return result;
    }
}


