using serilog_seq_demo.Adapter.MySql;
using serilog_seq_demo.DataClass.Record;
using serilog_seq_demo.DataServices.DAI;

namespace serilog_seq_demo.DataServices.DAO;

public class AdminDAO : IAdminDAO
{
    public IEnumerable<AdminRecord> GetAdminsByAuthority(int? authority)
    {
        return AdminAdapter.GetAdminsByAuthority(authority);
    }

    public AdminRecord? GetAdminById(string id)
    {
        return AdminAdapter.GetAdminById(id);
    }
}