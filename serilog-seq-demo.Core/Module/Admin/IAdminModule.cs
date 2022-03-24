using serilog_seq_demo.DataClass.Record;

namespace serilog_seq_demo.Core.Module.Admin;

public interface IAdminModule
{
    List<AdminRecord> GetAdminsByAuthority(int? authority);
    AdminRecord? GetAdminById(string id);
}