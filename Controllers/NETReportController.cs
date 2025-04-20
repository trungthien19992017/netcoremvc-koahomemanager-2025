using KOAHome.Attributes;
using KOAHome.EntityFramework;
using KOAHome.Helpers;
using KOAHome.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;
using static NuGet.Packaging.PackagingConstants;

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
    private readonly INetServiceService _netService;
    private readonly IConnectionService _con;

    public NETReportController(QLKCL_NEWContext db, ILogger<HsBookingsController> logger, IHsBookingTableService book, IHsBookingServiceService bookser, IReportEditorService re, IAttachmentService att, IHsCustomerService cus, IReportService report, IFormService form, IActionService action, IWidgetService widget, IDRDatasourceService datasrc, INetServiceService netService, IConnectionService con)
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
      _netService = netService;
      _con = con;
    }

    // GET: NETReportController
    public ActionResult Index()
    {
      return View();
    }

    // GET: NETReport/Viewer_Utility
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // Tắt cache mặc định cho action này nếu cần thiết
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
        // tra ve page loi neu khong tim thay report
        if (report == null)
        {
          return RedirectToAction("MiscError", "Pages", new { errorMessage = "Không tìm thấy bảng" });
        }
        // chuyen cau hinh report len giao dien de xu ly
        ViewBag.report = report;

        string? connectionString = null;
        //neu datasourceId la null thi lay connectionString mac dinh
        if (report.ContainsKey("DataSourceId"))
        {
          if (report["DataSourceId"] != null)
          {
            //lay connectionstring tu report de goi store
            connectionString = await _datasrc.GetConnectionString(Convert.ToInt32(report["DataSourceId"]));
          }
        }

        // khai bao cac du lieu report can su dung trong controller
        string? sqlContent = report.ContainsKey("SqlContent") ? Convert.ToString(report["SqlContent"]) : "";
        string? sqlDefaultContent = report.ContainsKey("SqlDefaultContent") ? Convert.ToString(report["SqlDefaultContent"]) : "";
        string? storeDRDisplay = report.ContainsKey("StoreDRDisplay") ? Convert.ToString(report["StoreDRDisplay"]) : "";

        if (string.IsNullOrWhiteSpace(sqlContent))
        {
          return RedirectToAction("MiscError", "Pages", new { errorMessage = "Không tồn tại store của bảng" });
        }

        // chuyen parameters cua bo loc thanh Idictionary<string, object>
        Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);

        // neu bo loc khong co va co store default filter thi lay du lieu mac dinh tu store
        if (objParameters.Count() == 0 && sqlDefaultContent != null && sqlDefaultContent != "")
        {
          var defaultFilter = await _report.NET_Report_GetDefaultFilter(null, sqlDefaultContent, connectionString);
          objParameters = defaultFilter != null ? new Dictionary<string, object>(defaultFilter) : new Dictionary<string, object>();
        }

        // chuyen bo loc len giao dien
        ViewBag.ListFilterValue = objParameters;

        // lay danh sach filter display cua report de xu ly
        var stopwatch = Stopwatch.StartNew();
        var filterListTask = _report.NET_Filter_WithReport_Get(ReportCode, null);
        stopwatch.Stop();
        _logger.LogInformation($"Query filterListTask executed in {stopwatch.ElapsedMilliseconds} ms");

        stopwatch.Restart();
        var displayListTask = _report.NET_Display_WithReport_Get(objParameters, ReportCode, null);
        stopwatch.Stop();
        _logger.LogInformation($"Query displayListTask executed in {stopwatch.ElapsedMilliseconds} ms");

        await Task.WhenAll(filterListTask, displayListTask);

        var filterList = await filterListTask;
        var displayList = await displayListTask;

        // tinh số cấp cha con của cột trong display report
        int displayParentLevelNum = _report.Display_GetReportMaxParentLevel(displayList);
        // chuyển cấu hình display lên giao diện để xử lý
        ViewBag.displayList = displayList;
        ViewBag.displayParentLevelNum = displayParentLevelNum;

        // xu ly bo loc filter
        ViewBag.ListFilterConfig = filterList;
        // doi voi cac filter co kieu select (select box, dropdownbox, tagbox,...), day cac bo select vao SelectListItem va đóng gói trong Dictionary để xử lý trên giao diện
        // Tạo Dictionary chứa SelectList cho từng dropdown (theo DynamicFieldName)
        var listFilterService = new Dictionary<string, List<SelectListItem>>();

        //Chuyển kết quả sang Dictionary<string, List<SelectListItem>>
        listFilterService = await _netService.NET_Service_GetListSelectListByFilter(filterList,objParameters);
        //  Gán danh sach select cho cac filter vào ViewBag
        ViewBag.DynamicServiceSelectOptions = listFilterService;

        // search
        stopwatch.Restart();
        var resultList = await _report.Report_search(objParameters, sqlContent, connectionString);
        stopwatch.Stop();
        _logger.LogInformation($"Query resultList executed in {stopwatch.ElapsedMilliseconds} ms");
        ViewBag.resultList = resultList;

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

    // GET: NETReport/Editor_Utility
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // Tắt cache mặc định cho action này nếu cần thiết
    public async Task<IActionResult> Editor_Utility([FromQuery] Dictionary<string, string> parameters, string? ReportCode, int? id)
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
        // tra ve page loi neu khong tim thay report
        if (report == null)
        {
          return RedirectToAction("MiscError", "Pages", new { errorMessage = "Không tìm thấy bảng" });
        }
        // chuyen cau hinh report len giao dien de xu ly
        ViewBag.report = report;

        string? connectionString = null;
        //neu datasourceId la null thi lay connectionString mac dinh
        if (report.ContainsKey("DataSourceId"))
        {
          if (report["DataSourceId"] != null)
          {
            //lay connectionstring tu report de goi store
            connectionString = await _datasrc.GetConnectionString(Convert.ToInt32(report["DataSourceId"]));
          }
        }

        // khai bao cac du lieu report can su dung trong controller
        string? sqlContent = report.ContainsKey("SqlContent") ? Convert.ToString(report["SqlContent"]) : "";
        string? sqlDefaultContent = report.ContainsKey("SqlDefaultContent") ? Convert.ToString(report["SqlDefaultContent"]) : "";
        string? storeDRDisplay = report.ContainsKey("StoreDRDisplay") ? Convert.ToString(report["StoreDRDisplay"]) : "";

        if (string.IsNullOrWhiteSpace(sqlContent))
        {
          return RedirectToAction("MiscError", "Pages", new { errorMessage = "Không tồn tại store của bảng" });
        }

        // chuyen parameters cua bo loc thanh Idictionary<string, object>
        Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);

        // neu bo loc khong co va co store default filter thi lay du lieu mac dinh tu store
        if (objParameters.Count() == 0 && sqlDefaultContent != null && sqlDefaultContent != "")
        {
          var defaultFilter = await _report.NET_Report_GetDefaultFilter(null, sqlDefaultContent, connectionString);
          objParameters = defaultFilter != null ? new Dictionary<string, object>(defaultFilter) : new Dictionary<string, object>();
        }

        // nếu tồn tại id thì add id vao parameter
        if (id != null)
        {
          objParameters.Add("Id", id ?? (object)DBNull.Value);
        }

        // chuyen bo loc len giao dien
        ViewBag.ListFilterValue = objParameters;

        // lay danh sach filter display cua report de xu ly
        var stopwatch = Stopwatch.StartNew();
        var filterListTask = _report.NET_Filter_WithReport_Get(ReportCode, null);
        stopwatch.Stop();
        _logger.LogInformation($"Query filterListTask executed in {stopwatch.ElapsedMilliseconds} ms");

        stopwatch.Restart();
        var displayListTask = _report.NET_Display_WithReport_Get(objParameters, ReportCode, null);
        stopwatch.Stop();
        _logger.LogInformation($"Query displayListTask executed in {stopwatch.ElapsedMilliseconds} ms");

        await Task.WhenAll(filterListTask, displayListTask);

        var filterList = await filterListTask;
        var displayList = await displayListTask;

        // tinh số cấp cha con của cột trong display report
        int displayParentLevelNum = _report.Display_GetReportMaxParentLevel(displayList);
        // chuyển cấu hình display lên giao diện để xử lý
        ViewBag.displayList = displayList;
        ViewBag.displayParentLevelNum = displayParentLevelNum;

        // xu ly bo loc filter
        ViewBag.ListFilterConfig = filterList;
        // doi voi cac filter co kieu select (select box, dropdownbox, tagbox,...), day cac bo select vao SelectListItem va đóng gói trong Dictionary để xử lý trên giao diện
        // Tạo Dictionary chứa SelectList cho từng dropdown (theo DynamicFieldName)
        var listFilterService = new Dictionary<string, List<SelectListItem>>();

        //Chuyển kết quả sang Dictionary<string, List<SelectListItem>>
        listFilterService = await _netService.NET_Service_GetListSelectListByFilter(filterList, objParameters);
        //  Gán danh sach select cho cac filter vào ViewBag
        ViewBag.DynamicServiceSelectOptions = listFilterService;


        // doi voi cac display co kieu select (select box, dropdownbox, tagbox,...), day cac bo select vao SelectListItem va đóng gói trong Dictionary để xử lý trên giao diện
        // Tạo Dictionary chứa SelectList cho từng dropdown (theo DynamicFieldName)
        var listDisplayService = new Dictionary<string, List<SelectListItem>>();

        //Chuyển kết quả sang Dictionary<string, List<SelectListItem>>
        listDisplayService = await _netService.NET_Service_GetListSelectListByDisplay(displayList, objParameters);
        //  Gán danh sach select cho cac display vào ViewBag
        ViewBag.EditorDynamicServiceSelectOptions = listDisplayService;

        // search
        stopwatch.Restart();
        var resultList = await _report.Report_search(objParameters, sqlContent, connectionString);
        stopwatch.Stop();
        _logger.LogInformation($"Query resultList executed in {stopwatch.ElapsedMilliseconds} ms");
        ViewBag.resultList = resultList;

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


    // POST: /report/editor-utility/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost("/report/editor-utility/{ReportCode}/{id?}")]
    //[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editor_Utility(string? ReportCode, int? id, [FromForm] IFormCollection form)
    {
      try
      {
        // reset tempdata error message
        TempData["ErrorMessage"] = null;

        // Nếu bạn cần redirect (ví dụ sau khi lưu), có thể dùng:
        // Tách các form input có tiền tố "q_" vì tiền tố q_ là các query param từ link
        string queryString = ParseDataHelper.GetQueryStringFromForm(form);
        string currentPath = HttpContext.Request.Path;

        // Tách lại query param gốc từ form input "q_" để lọc dữ liệu khi xử lý
        var queryParamerter = form
            .Where(kv => kv.Key.StartsWith("q_"))
            .ToDictionary(
                kv => kv.Key.Substring(2),
                kv => (object)kv.Value.ToString()
            );

        // xử lý form để loại các tiền tố q_ ra khỏi Key
        form = ParseDataHelper.RemovePrefix_FromFormKey(form);

        // lay thong tin report, va danh sach filter display cua report de xu ly
        var report = await _report.NET_Report_Get(ReportCode);
        // tra ve page loi neu khong tim thay report
        if (report == null)
        {
          TempData["ErrorMessage"] = "Không tìm thấy bảng";
          return Redirect($"{currentPath}?{queryString}");
        }

        string? connectionString = null;
        //neu datasourceId la null thi lay connectionString mac dinh
        if (report.ContainsKey("DataSourceId"))
        {
          if (report["DataSourceId"] != null)
          {
            //lay connectionstring tu report de goi store
            connectionString = await _datasrc.GetConnectionString(Convert.ToInt32(report["DataSourceId"]));
          }
        }

        // khai bao cac du lieu report can su dung trong controller
        string? sqlEditContent = report.ContainsKey("SqlEditContent") ? Convert.ToString(report["SqlEditContent"]) : "";

        if (string.IsNullOrWhiteSpace(sqlEditContent))
        {
          TempData["ErrorMessage"] = "Không tồn tại store cập nhật dữ liệu của bảng";
          return Redirect($"{currentPath}?{queryString}");
        }

        /////////////////////////////////////// xử lý lưu editor ////////////////////////////
        // Convert the IFormCollection to a dictionary of strings
        var formData = form.ToDictionary(
                        pair => pair.Key,
                        pair => (object)pair.Value.ToString()  // Ensure each value is a string (flatten StringValues)
                    );

        //// gui form data len view de hien thi
        //ViewBag.formData = formData;

        //xu ly report editor
        // Dictionary để nhóm dữ liệu theo số thứ tự [n]
        // Chuyển đổi dữ liệu sang JSON (loc du lieu form tra ve lay du lieu grid va chuyen thanh json)
        string reportJsonData = await _re.ExtractGridDataToJson(form);
        //end xu ly report form
        var reportResultList = await _re.ReportEditor_Json_Update(queryParamerter, id, reportJsonData, sqlEditContent, connectionString);
        //kiem tra ton tai error message
        // Kiểm tra và nối giá trị của ErrorMessage
        if (_con.CheckForErrors(reportResultList, out string errorMessage))
        {
          TempData["ErrorMessage"] = errorMessage;
          return Redirect($"{currentPath}?{queryString}");
        }
        // khong tra ve Id, cung khong tra ve error message thi bao loi chua tra ve id
        else
        {
          ViewBag.success = "Xử lý thành công"; // Gán vào ViewBag
          return Redirect($"{currentPath}?{queryString}");
        }
      }
      catch (Exception ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching booking service info.");
        // Optionally, return an error view
        return View("Error");
      }
    }
  }
}
