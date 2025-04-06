using KOAHome.EntityFramework;
using KOAHome.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SqlServer.Server;
using OfficeOpenXml;

namespace KOAHome.Controllers
{
  public class ChiPhiController : Controller
  {
    private readonly ILogger<HsBookingsController> _logger;
    private readonly QLKCL_NEWContext _db;
    private readonly IHsBookingTableService _book;
    private readonly IHsBookingServiceService _bookser;
    private readonly IReportEditorService _re;
    private readonly IAttachmentService _att;
    private readonly IHsCustomerService _cus;
    private readonly IReportService _report;
    private readonly IFormService _form;
    private readonly IActionService _action;

    public ChiPhiController(QLKCL_NEWContext db, ILogger<HsBookingsController> logger, IHsBookingTableService book, IHsBookingServiceService bookser, IReportEditorService re, IAttachmentService att, IHsCustomerService cus, IReportService report, IFormService form, IActionService action)
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

    private bool CheckForErrors(List<dynamic> resultList, out string errorMessage)
    {
      var errorMessages = resultList
          .Where(item => ((IDictionary<string, object>)item).ContainsKey("ErrorMessage"))
          .Select(item => item.ErrorMessage.ToString())
          .ToList();

      if (errorMessages.Any())
      {
        errorMessage = string.Join(", ", errorMessages);
        return true;
      }

      errorMessage = null;
      return false;
    }

    // GET: ChiPhiController
    public async Task<IActionResult> Index([FromQuery] Dictionary<string, string> parameters, int page = 1, int pageSize = 20)
    {
      try
      {
        // reset tempdata error message
        TempData["ErrorMessage"] = null;

        // chuyen parameters thanh Idictionary<string, object>
        Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);

        //Phân trang
        // search
        var (resultList, totalRecord, maxPage, totalPage) = await _report.Report_Pagination_search("HS_ChiPhi_search", null, objParameters, page, pageSize);
        ViewBag.gridData_Store = resultList;
        // gia tri pham trang tra ve
        ViewBag.Total = totalRecord;
        ViewBag.Page = page;
        ViewBag.TotalPage = totalPage;
        ViewBag.MaxPage = maxPage;
        ViewBag.First = 1;
        ViewBag.Last = totalPage;
        ViewBag.Next = page + 1;
        ViewBag.Prev = page - 1;

        // neu co loi tu action POST tra ve thi bao loi
        if (TempData["ErrorMessage"] != null)
        {
          ViewBag.ErrorMessage = TempData["ErrorMessage"];
        }
        else
        {
          //khai bao success
          ViewBag.success = "Thành công";
        }

        return View();
      }
      catch (Exception ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching booking service info.");
        // Optionally, return an error view
        return View("Error");
      }
    }

    // POST: HsBookings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [FromForm] IFormCollection form)
    {
      // Convert the IFormCollection to a dictionary of strings
      var formData = form.ToDictionary(
                      pair => pair.Key,
                      pair => (object)pair.Value.ToString()  // Ensure each value is a string (flatten StringValues)
                  );

      // gui form data len view de hien thi
      ViewBag.formData = formData;

      //xu ly report editor
      // Dictionary để nhóm dữ liệu theo số thứ tự [n]
      // Chuyển đổi dữ liệu sang JSON (loc du lieu form tra ve lay du lieu grid va chuyen thanh json)
      string reportJsonData = await _re.ExtractGridDataToJson(form);
      //end xu ly report form
      var reportResultList = await _re.ReportEditor_Json_Update(id, reportJsonData, "HS_ChiPhi_Json_ups", null);
      //kiem tra ton tai error message
      // Kiểm tra và nối giá trị của ErrorMessage
      if (CheckForErrors(reportResultList, out string errorMessage))
      {
        //ViewBag.ErrorMessage = errorMessage;
        TempData["ErrorMessage"] = errorMessage;
        return RedirectToAction("Index");
      }
      // khong tra ve Id, cung khong tra ve error message thi bao loi chua tra ve id
      else
      {
        ViewBag.success = "Xử lý thành công"; // Gán vào ViewBag
        return RedirectToAction("Index");
      }
    }
  }
}
