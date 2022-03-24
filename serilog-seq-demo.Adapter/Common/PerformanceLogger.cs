using Microsoft.Extensions.Logging;
using serilog_seq_demo.Common.Utility;

namespace serilog_seq_demo.Adapter.Common;

internal class PerformanceLogger
{
    /// <summary>
    /// 寫入執行效能記錄
    /// </summary>
    /// <param name="db">資料庫</param>
    /// <param name="command">執行命令</param>
    /// <param name="runTime">執行時間</param>
    /// <param name="param">執行參數</param>
    internal static void Write(Database db, string command, TimeSpan runTime, Dictionary<string, string?>? param)
    {
        ILogger? logger = ServiceLocator.Current.GetInstance<ILogger<PerformanceLogger>>();

        logger?.LogInformation(
            "{Elapsed} {Db} {Command} {Param}",
            runTime,
            db,
            command.Replace("\r\n", " "),
            string.Join(",", param?.Select(p => $"{p.Key}:{p.Value}") ?? new List<string>())
        );
    }
}

/// <summary>
/// 資料庫列舉
/// </summary>
internal enum Database
{
    /// <summary>
    /// MySql
    /// </summary>
    MySql
}