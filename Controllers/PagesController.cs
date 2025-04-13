using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcFull.Models;

namespace AspnetCoreMvcFull.Controllers;

public class PagesController : Controller
{
  public IActionResult AccountSettings() => View();
  public IActionResult AccountSettingsConnections() => View();
  public IActionResult AccountSettingsNotifications() => View();
  public IActionResult MiscError(string errorMessage)
  {
    ViewBag.errorMessage = errorMessage;
    return View();
  }
  public IActionResult MiscUnderMaintenance(string errorMessage)
  {
    ViewBag.errorMessage = errorMessage;
    return View();
  }
}
