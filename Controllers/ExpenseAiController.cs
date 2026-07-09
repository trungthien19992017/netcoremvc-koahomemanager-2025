using KOAHome.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspnetCoreMvcFull.Controllers
{
  public class ExpenseAiController : Controller
  {
    private readonly IExpenseIconService _expenseIconService;

    public ExpenseAiController(IExpenseIconService expenseIconService)
    {
      _expenseIconService = expenseIconService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Index()
    {
      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Classify([FromBody] ClassifyRequest req)
    {
      var result = await _expenseIconService.ClassifyAsync(
          req.ExpenseName,
          string.IsNullOrEmpty(req.Provider) ? "gemini" : req.Provider,
          string.IsNullOrEmpty(req.Model) ? "gemini-2.0-flash" : req.Model
      );
      return Json(result);
    }

    public class ClassifyRequest
    {
      public string ExpenseName { get; set; } = "";
      public string? Provider { get; set; }
      public string? Model { get; set; }
    }
  }
}
