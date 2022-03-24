using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace serilog_seq_demo.Adapter.Common;

public static class DBConfig
{
    private static string? _myConnString = string.Empty;

    private static IServiceCollection _serviceCollection;

    /// <summary>
    /// 於 Program 注入 ServiceCollection
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static void Configure(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    private static IConfiguration? Configuration => _serviceCollection.BuildServiceProvider().GetService<IConfiguration>();

    private static string GetConnection(string name) => Configuration.GetConnectionString(name);

    internal static string MyConnString
    {
        get
        {
            if (!string.IsNullOrEmpty(_myConnString)) return _myConnString;

            _myConnString =  GetConnection(nameof(MyConnString));
            if (string.IsNullOrEmpty(_myConnString)) throw new ArgumentNullException(nameof(MyConnString), $"{nameof(MyConnString)} is not setting.");

            return _myConnString;
        }
    }
}