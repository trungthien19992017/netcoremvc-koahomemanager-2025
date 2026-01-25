using CommunityToolkit.HighPerformance;
using KOAHome.EntityFramework;
using KOAHome.Helpers;
using KOAHome.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspnetCoreMvcFull.Controllers;

public class HomestayAiController : Controller
{
  private readonly Func<string, IAiService> _aiServiceFactory;
  private readonly QLKCL_NEWContext _db;
  public HomestayAiController(ILogger<HomestayAiController> logger, Func<string, IAiService> aiServiceFactory, QLKCL_NEWContext db)
  {
    _aiServiceFactory = aiServiceFactory;
    _db = db;
  }

  [HttpGet]
  public async Task<IActionResult> Index(int bookingID = 2935)
  {
    var booking = _db.HsBookings.FirstOrDefault(p => p.Bookingid == bookingID);
    var customer = _db.HsCustomers.FirstOrDefault(p => p.Customerid == booking.Customerid);
    string fullName = $"{customer.Firstname} {customer.Lastname}";
    string shortName = FormatHelper.GetLogoShortName(fullName);

    ViewBag.BookingID = bookingID;
    ViewBag.FullName = fullName;
    ViewBag.ShortName = shortName;

    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Index(ChatRequest req)
  {
    var booking = _db.HsBookings.FirstOrDefault(p => p.Bookingid == req.BookingID);
    string phoneNumber = _db.HsCustomers.FirstOrDefault(p => p.Customerid == booking.Customerid).Phonenumber;

    var aiService = _aiServiceFactory(req.selectedProvider);
    //var prompt = _ai.BuildGuestPrompt(req.BookingID, req.Message);
    var prompt = aiService.BuildGuestPromptByPhone(phoneNumber, req.Message);
    var reply = await aiService.AskAsync(prompt, req.selectedModel);
    return Content(reply);
  }

  public class ChatRequest
  {
    public string? PhoneNumber { get; set; }
    public int? BookingID { get; set; } = 2941;
    public string Message { get; set; } = "";
    public string selectedProvider { get; set; }
    public string selectedModel { get; set; }
  }

  public class ChatResponse
  {
    public string Reply { get; set; } = "";
  }
}
