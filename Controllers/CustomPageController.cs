using CommunityToolkit.HighPerformance;
using KOAHome.EntityFramework;
using KOAHome.Helpers;
using KOAHome.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspnetCoreMvcFull.Controllers;

public class CustomPageController : Controller
{
  public CustomPageController()
  {
  }

  [HttpGet]
  public async Task<IActionResult> SNTPortfolio()
  {
    return View();
  }
}
