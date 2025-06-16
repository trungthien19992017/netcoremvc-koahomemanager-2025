using Microsoft.AspNetCore.Mvc;

namespace KOAHome.Controllers
{
  public class NETChatController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
