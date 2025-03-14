using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcFull.Models;
using KOAHome.EntityFramework;
using KOAHome.Services;
using Newtonsoft.Json.Linq;

namespace AspnetCoreMvcFull.Controllers;

public class DashboardsController : Controller
{
  private readonly ILogger<DashboardsController> _logger;
  private readonly QLKCL_NEWContext _db;
  private readonly IHsCustomerService _cus;


  public DashboardsController(ILogger<DashboardsController> logger, IHsCustomerService cus)
  {
    _logger = logger;
    _cus = cus;
  }

  public async Task<IActionResult> Index()
  {
    return View();
  }


  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}
