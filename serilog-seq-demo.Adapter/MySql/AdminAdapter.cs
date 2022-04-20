using System.Data;
using MySql.Data.MySqlClient;
using serilog_seq_demo.Adapter.Common;
using serilog_seq_demo.DataClass.Record;

namespace serilog_seq_demo.Adapter.MySql;

public static class AdminAdapter
{
    public static IEnumerable<AdminRecord> GetAdminsByAuthority(int? authority)
    {
        // var parameters = new MySqlParameter[]
        // {
        //     new("@authority", MySqlDbType.Int16) { Value = authority },
        // };

        // return MySqlProxy.QueryCollection<AdminRecord>(DBConfig.MyConnString, CommandType.Text, Command.Admin.GetAdminsByAuthority, parameters);

        var fake = new List<AdminRecord>()
        {
            new AdminRecord() { Account = "art", Authority = 0, Id = "art", Name = "art" },
            new AdminRecord() { Account = "bob", Authority = 0, Id = "bob", Name = "bob" },
        };
        return fake;
    }

    public static AdminRecord? GetAdminById(string id)
    {
        // var parameters = new MySqlParameter[]
        // {
        //     new("@id", MySqlDbType.VarChar) { Value = id },
        // };
        //
        // return MySqlProxy.Query<AdminRecord>(DBConfig.MyConnString, CommandType.Text, Command.Admin.GetAdminById, parameters);
        return new AdminRecord() { Account = "art", Authority = 0, Id = "art", Name = "art" };

    }
}