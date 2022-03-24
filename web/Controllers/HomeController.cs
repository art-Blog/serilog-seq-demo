using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using seq_demo.Controllers.Base;
using seq_demo.Models;

namespace seq_demo.Controllers;

public class HomeController : BaseController
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}