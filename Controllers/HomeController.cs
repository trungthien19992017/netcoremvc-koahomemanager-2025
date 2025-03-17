using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcFull.Models;
using KOAHome.EntityFramework;
using KOAHome.Services;

namespace AspnetCoreMvcFull.Controllers;

public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;
  private readonly QLKCL_NEWContext _db;
  private readonly IHsBookingTableService _book;
  private readonly IHsBookingServiceService _bookser;
  private readonly IReportEditorService _re;
  private readonly IAttachmentService _att;
  private readonly IHsCustomerService _cus;
  private readonly IReportService _report;
  private readonly IFormService _form;
  private readonly IActionService _action;

  public HomeController(QLKCL_NEWContext db, ILogger<HomeController> logger, IHsBookingTableService book, IHsBookingServiceService bookser, IReportEditorService re, IAttachmentService att, IHsCustomerService cus, IReportService report, IFormService form, IActionService action)
  {
    _db = db;
    _logger = logger;
    _book = book;
    _bookser = bookser;
    _re = re;
    _att = att;
    _cus = cus;
    _report = report;
    _form = form;
    _action = action;
  }

  public IActionResult Index()
    {
        return View();
    }

  // kiem tra dieu kien menu top de load menu left
  public IActionResult LoadVerticalNavbar(string? menu)
  {
    ViewBag.MenuCode = menu ?? "";
    if (menu == "Booking")
    {
      return PartialView("Sections/Menu/_VerticalMenu_Booking");
    }
    return PartialView("Sections/Menu/_VerticalMenu");
  }

  // POST: Home/Action_Confirmed/5
  [HttpPost]
  public async Task<IActionResult> Action_Confirmed([FromForm] IFormCollection form)
  {
    try
    {
      // lay gia tri tu form gui len
      // Convert the IFormCollection to a dictionary of strings
      var formData = form.ToDictionary(
                      pair => pair.Key,
                      pair => (object)pair.Value.ToString()  // Ensure each value is a string (flatten StringValues)
                  );

      // lay gia tri sql store tu ajax gui len
      string? sqlstore = formData.ContainsKey("sqlstore") ? formData["sqlstore"].ToString() : null;
      if (sqlstore == null)
      {
        return Json(new { success = false, errorMessage = "Không tìm thấy store." });
      }

      // Kiểm tra và xóa key "sqlstore" nếu tồn tại
      if (formData.ContainsKey("sqlstore"))
      {
        formData.Remove("sqlstore");
      }


      // xu ly luu form
      var resultList = await _action.Action_store(formData, sqlstore, null);
      //kiem tra du lieu success tra ve
      var success_return = resultList
      .Where(item => ((IDictionary<string, object>)item).ContainsKey("Success"))
      .Select(item => ((IDictionary<string, object>)item)["Success"])
      .FirstOrDefault(); // Lọc ra những phần tử có Success

      //kiem tra du lieu error message tra ve
      var errormessage_return = resultList
      .Where(item => ((IDictionary<string, object>)item).ContainsKey("ErrorMessage"))
      .Select(item => ((IDictionary<string, object>)item)["ErrorMessage"])
      .FirstOrDefault(); // Lọc ra những phần tử có ErrorMessage

      bool success = false;
      string? errorMessage = null;
      // neu co gia tri success tra ve thi xu ly tiep, khong thi bao loi
      if (success_return != null && bool.TryParse(success_return.ToString(), out bool issuccess))
      {
        success = (bool)success_return;
        // kiem tra co error message tra ve hay khong
        if (errormessage_return != null && errormessage_return.ToString() != "")
        {
          errorMessage = errormessage_return.ToString();
        }
        // tra ve json
        if (success == true)
        {
          return Json(new { success = true });
        }
        else
        {
          return Json(new { success = false, errorMessage = errorMessage ?? "Có lỗi trong quá trình xử lý" });
        }
      }
      else
      {
        return Json(new { success = false, errorMessage = "Store chưa trả về giá trị success." });
      }
    }
    catch (Exception ex)
    {
      return Json(new { success = false, errorMessage = ex.Message });
    }
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
