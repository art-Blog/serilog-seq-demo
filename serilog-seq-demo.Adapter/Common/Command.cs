namespace serilog_seq_demo.Adapter.Common;

internal static class Command
{
    internal struct Admin
    {
        internal const string GetAdminsByAuthority = @"
select a.AdminID as 'Id', a.Account, a.XingMing as 'Name', a.Authority
from admins as a
where a.authority = @authority
";

        internal const string GetAdminById = @"
select a.AdminID as 'Id', a.Account, a.XingMing as 'Name', a.Authority
from admins as a
where a.AdminID = @id
limit 1
";
    }
}