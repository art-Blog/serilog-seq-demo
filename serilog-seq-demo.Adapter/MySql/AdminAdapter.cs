using System.Data;
using MySql.Data.MySqlClient;
using serilog_seq_demo.Adapter.Common;
using serilog_seq_demo.DataClass.Record;

namespace serilog_seq_demo.Adapter.MySql;

public static class AdminAdapter
{
    public static IEnumerable<AdminRecord> GetAdminsByAuthority(int? authority)
    {
        var parameters = new MySqlParameter[]
        {
            new("@authority", MySqlDbType.Int16) { Value = authority },
        };

        return MySqlProxy.QueryCollection<AdminRecord>(DBConfig.MyConnString, CommandType.Text, Command.Admin.GetAdminsByAuthority, parameters);
    }

    public static AdminRecord? GetAdminById(string id)
    {
        var parameters = new MySqlParameter[]
        {
            new("@id", MySqlDbType.VarChar) { Value = id },
        };

        return MySqlProxy.Query<AdminRecord>(DBConfig.MyConnString, CommandType.Text, Command.Admin.GetAdminById, parameters);
    }
}