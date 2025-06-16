using Microsoft.AspNetCore.Mvc;

namespace KOAHome.Controllers
{
  public class NETCalendarController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
