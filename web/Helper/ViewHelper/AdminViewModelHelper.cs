using seq_demo.Models.Admin;
using serilog_seq_demo.DataClass.Record;

namespace seq_demo.Helper.ViewHelper;

public static class AdminViewModelHelper
{
    public static AdminDetailViewModel ConvertDetail(AdminRecord? admin)
    {
        return new AdminDetailViewModel()
        {
            Admin = admin
        };
    }
}