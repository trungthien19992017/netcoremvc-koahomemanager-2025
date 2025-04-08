using KOAHome.EntityFramework;
using KOAHome.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KOAHome.Controllers
{
  public class NETReportController : Controller
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
    private readonly IWidgetService _widget;
    private readonly IDRDatasourceService _datasrc;

    public NETReportController(QLKCL_NEWContext db, ILogger<HsBookingsController> logger, IHsBookingTableService book, IHsBookingServiceService bookser, IReportEditorService re, IAttachmentService att, IHsCustomerService cus, IReportService report, IFormService form, IActionService action, IWidgetService widget, IDRDatasourceService datasrc)
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
      _widget = widget;
      _datasrc = datasrc;
    }

    // cac phuong thuc con de toi uu xu ly
    private async Task HandleFiles(string objectTypeCode, IFormCollection? form, int? id)
    {
      if (form != null)
      {
        if (form.Files.Any())
        {
          List<string> fileUrls = await _att.UpdateFiles(form); // Gọi service để lưu file
          ViewBag.fileUrls = fileUrls; // Truyền danh sách file về View (neu co)
        }
        else if (id != null)
        {
          List<string> fileUrls = await _att.GetFiles(id, objectTypeCode); // Gọi service để get file tu objectId va ObjectTypeCode
          ViewBag.fileUrls = fileUrls; // Truyền danh sách file về View (neu co)

        }
      }
      else if (id != null)
      {
        List<string> fileUrls = await _att.GetFiles(id, objectTypeCode); // Gọi service để get file tu objectId va ObjectTypeCode
        ViewBag.fileUrls = fileUrls; // Truyền danh sách file về View (neu co)
      }
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

    // GET: NETReportController
    public ActionResult Index()
    {
      return View();
    }

    // GET: NETReport/Viewer_Utility
    public async Task<IActionResult> Viewer_Utility([FromQuery] Dictionary<string, string> parameters, string? ReportCode)
    {
      try
      {
        // neu không trả về report code thì chuyển sang link lỗi
        if (ReportCode == null)
        {
          return RedirectToAction("MiscError", "Pages");
        }
        ViewBag.ReportCode = ReportCode;

        // lay thong tin report, va danh sach filter display cua report de xu ly
        var report = await _report.NET_Report_Get(ReportCode);
        var filterList = await _report.NET_Filter_WithReport_Get(ReportCode, null);
        var displayList = await _report.NET_Display_WithReport_Get(ReportCode, null);

        string connectionString = await _datasrc.GetConnectionString(report.DataSourceId);

        // Kiểm tra xem có key "isActive" không, nếu không có thì gán giá trị mặc định (null hoặc giá trị khác)
        string isActive = parameters.ContainsKey("isActive") ? parameters["isActive"] : "";

        // Service cho filter hoat dong
        ViewBag.IsActiveOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Có" , Selected = isActive == "1"},
            new SelectListItem { Value = "2", Text = "Không" , Selected = isActive == "2"}
        };

        // chuyen parameters thanh Idictionary<string, object>
        Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);

        //Phân trang
        // search
        var resultList = await _report.Report_search(objParameters, "HS_Booking1_search", connectionString);
        ViewBag.listbook_store = resultList;

        // khai bao service lien quan
        var service_parameters = new Dictionary<string, object>();
        ViewBag.listbookser_info_store = await _report.Report_search(service_parameters, "HS_BookingService_Info", null);

        //khai bao success
        ViewBag.success = "Thành công";


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

    // GET: NETReportController/Details/5
    public ActionResult Details(int id)
    {
      return View();
    }

    // GET: NETReportController/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: NETReportController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
      try
      {
        return RedirectToAction(nameof(Index));
      }
      catch
      {
        return View();
      }
    }

    // GET: NETReportController/Edit/5
    public ActionResult Edit(int id)
    {
      return View();
    }

    // POST: NETReportController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
      try
      {
        return RedirectToAction(nameof(Index));
      }
      catch
      {
        return View();
      }
    }

    // GET: NETReportController/Delete/5
    public ActionResult Delete(int id)
    {
      return View();
    }

    // POST: NETReportController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
      try
      {
        return RedirectToAction(nameof(Index));
      }
      catch
      {
        return View();
      }
    }
  }
}
