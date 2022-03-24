using Microsoft.AspNetCore.Mvc;
using seq_demo.Controllers.Base;
using seq_demo.Helper.ViewHelper;
using seq_demo.Models;
using seq_demo.Models.Admin;
using Serilog;
using serilog_seq_demo.Core.Module.Admin;
using serilog_seq_demo.DataClass.Record;

namespace seq_demo.Controllers;

public class AdminController : BaseController
{
    private readonly ILogger<AdminController> _logger;
    private readonly IDiagnosticContext _diagnosticContext;
    private readonly IAdminModule _adminModule;

    public AdminController(ILogger<AdminController> logger, IDiagnosticContext diagnosticContext, IAdminModule adminModule)
    {
        _logger = logger;
        _diagnosticContext = diagnosticContext;
        _adminModule = adminModule;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult GetAdminsByAuthority(AdminRequest req)
    {
        var result = _adminModule.GetAdminsByAuthority(req.Authority);
        var response = new JsonResponse<List<AdminRecord>>(result);

        _logger.LogDebug("/Admin/GetAdminsByAuthority, Input:{@Request} Output:{@Response}", req, response);
        return Json(response);
    }

    public IActionResult Detail(string id)
    {
        var admin = _adminModule.GetAdminById(id);
        var model = AdminViewModelHelper.ConvertDetail(admin);

        return View(model);
    }
}