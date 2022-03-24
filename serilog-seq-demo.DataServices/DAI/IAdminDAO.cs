using serilog_seq_demo.DataClass.Record;

namespace serilog_seq_demo.DataServices.DAI;

public interface IAdminDAO
{
    IEnumerable<AdminRecord> GetAdminsByAuthority(int? authority);
    AdminRecord? GetAdminById(string id);
}