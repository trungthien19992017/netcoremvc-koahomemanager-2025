using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KOAHome.Controllers
{
  [Route("health")]
  public class HealthController : Controller
  {
    [HttpGet]
    public IActionResult Get() => Ok("Healthy");
  }
}
