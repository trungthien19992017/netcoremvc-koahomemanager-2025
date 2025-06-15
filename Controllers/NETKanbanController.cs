using Microsoft.AspNetCore.Mvc;

namespace KOAHome.Controllers
{
  public class NETKanbanController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
