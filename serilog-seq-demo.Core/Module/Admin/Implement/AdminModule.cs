using serilog_seq_demo.DataClass.Record;
using serilog_seq_demo.DataServices.DAI;

namespace serilog_seq_demo.Core.Module.Admin.Implement;

public class AdminModule : IAdminModule
{
    private readonly IAdminDAO _adminDAO;

    public AdminModule(IAdminDAO adminDAO)
    {
        _adminDAO = adminDAO;
    }
    public List<AdminRecord> GetAdminsByAuthority(int? authority)
    {
        return _adminDAO.GetAdminsByAuthority(authority).ToList();
    }

    public AdminRecord? GetAdminById(string id)
    {
        return _adminDAO.GetAdminById(id);
    }
}