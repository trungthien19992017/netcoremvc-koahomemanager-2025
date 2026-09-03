using AspnetCoreMvcFull.Models;
using KOAHome.EntityFramework;
using KOAHome.Helpers;
using KOAHome.Models;
using KOAHome.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using static NuGet.Packaging.PackagingConstants;

namespace KOAHome.Controllers
{
  public class NETReportController : Controller
  {
    private readonly ILogger<NETReportController> _logger;
    private readonly QLKCL_NEWContext _db;
    private readonly IReportEditorService _re;
    private readonly IAttachmentService _att;
    private readonly IReportService _report;
    private readonly IFormService _form;
    private readonly IActionService _action;
    private readonly IWidgetService _widget;
    private readonly IDRDatasourceService _datasrc;
    private readonly INetServiceService _netService;
    private readonly IConnectionService _con;

    public NETReportController(QLKCL_NEWContext db, ILogger<NETReportController> logger, IReportEditorService re, IAttachmentService att, IReportService report, IFormService form, IActionService action, IWidgetService widget, IDRDatasourceService datasrc, INetServiceService netService, IConnectionService con)
    {
      _db = db;
      _logger = logger;
      _re = re;
      _att = att;
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
    [Authorize]
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // Tắt cache mặc định cho action này nếu cần thiết
    public async Task<IActionResult> Viewer_Utility(string? ReportCode)
    {
      try
      {
        // neu không trả về report code thì chuyển sang link lỗi
        if (ReportCode == null)
        {
          return RedirectToAction("MiscError", "Pages", new { errorMessage = "Không tồn tại mã danh sách" });
        }
        ViewData["ReportCode"] = ReportCode;

        // Lấy dynamic query parameters
        var parameters = Request.Query;

        // lay thong tin report, va danh sach filter display cua report de xu ly
        var report = await _report.NET_Report_Get(ReportCode);
        // tra ve page loi neu khong tim thay report
        if (report == null)
        {
          return RedirectToAction("MiscError", "Pages", new { errorMessage = "Không tìm thấy bảng" });
        }
        // chuyen cau hinh report len giao dien de xu ly
        ViewData["report"] = report;

        string? connectionString = null;
        //neu datasourceId la null thi lay connectionString mac dinh
        if (report.ContainsKey("datasourceid"))
        {
          if (report["datasourceid"] != null)
          {
            //lay connectionstring tu report de goi store
            connectionString = await _datasrc.GetConnectionString(Convert.ToInt32(report["datasourceid"]));
          }
        }

        // khai bao cac du lieu report can su dung trong controller
        string? sqlContent = report.ContainsKey("sqlcontent") ? Convert.ToString(report["sqlcontent"]) : "";
        string? sqlDefaultContent = report.ContainsKey("sqldefaultcontent") ? Convert.ToString(report["sqldefaultcontent"]) : "";
        string? storeDRDisplay = report.ContainsKey("storedrdisplay") ? Convert.ToString(report["storedrdisplay"]) : "";

        if (string.IsNullOrWhiteSpace(sqlContent))
        {
          return RedirectToAction("MiscError", "Pages", new { errorMessage = "Không tồn tại store của bảng" });
        }

        // chuyen parameters cua bo loc thanh Idictionary<string, object>
        //Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)(kvp.Value.Count > 1 ? kvp.Value.Split(',', StringSplitOptions.RemoveEmptyEntries) : kvp.Value.ToString()));

        // With this corrected version:  
        Dictionary<string, object> objParameters = parameters.ToDictionary(
            kvp => kvp.Key,
            kvp => (object)(kvp.Value.ToString())
        );
        // neu bo loc khong co va co store default filter thi lay du lieu mac dinh tu store
        if (objParameters.Count() == 0 && sqlDefaultContent != null && sqlDefaultContent != "")
        {
          var defaultFilter = await _report.NET_Report_GetDefaultFilter(null, sqlDefaultContent, connectionString);
          objParameters = defaultFilter != null ? new Dictionary<string, object>(defaultFilter) : new Dictionary<string, object>();
        }

        // chuyen bo loc len giao dien
        ViewData["ListFilterValue"] = objParameters;

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
        ViewData["displayList"] = displayList;
        ViewData["displayParentLevelNum"] = displayParentLevelNum;

        // xu ly bo loc filter
        ViewData["ListFilterConfig"] = filterList;
        // doi voi cac filter co kieu select (select box, dropdownbox, tagbox,...), day cac bo select vao SelectListItem va đóng gói trong Dictionary để xử lý trên giao diện
        // Tạo Dictionary chứa SelectList cho từng dropdown (theo DynamicFieldName)
        var listFilterService = new Dictionary<string, List<SelectListItem>>();

        //Chuyển kết quả sang Dictionary<string, List<SelectListItem>>
        listFilterService = await _netService.NET_Service_GetListSelectListByFilter(filterList, objParameters);
        //  Gán danh sach select cho cac filter vào ViewBag
        ViewData["DynamicServiceSelectOptions"] = listFilterService;

        // doi voi cac display co kieu select (select box, dropdownbox, tagbox,...), day cac bo select vao SelectListItem va đóng gói trong Dictionary để xử lý trên giao diện
        // Tạo Dictionary chứa SelectList cho từng dropdown (theo DynamicFieldName)
        var listDisplayService = new Dictionary<string, List<SelectListItem>>();

        //Chuyển kết quả sang Dictionary<string, List<SelectListItem>>
        listDisplayService = await _netService.NET_Service_GetListSelectListByDisplay(displayList, objParameters);
        //  Gán danh sach select cho cac display vào ViewBag
        ViewData["EditorDynamicServiceSelectOptions"] = listDisplayService;

        // lấy danh sách action list detail theo object code là report code
        var actionlistdetailList = await _action.NET_ActionListDetail_WithObject_Get(ReportCode, null, "REPORT");
        // chuyển action list detail lên giao diện để xử lý
        ViewData["actionlistdetailList"] = actionlistdetailList;

        // search
        stopwatch.Restart();
        var resultList = await _report.Report_search(objParameters, sqlContent, connectionString);
        stopwatch.Stop();
        _logger.LogInformation($"Query resultList executed in {stopwatch.ElapsedMilliseconds} ms");
        ViewData["resultList"] = resultList;

        ViewData["TableRowsHtml"] = await _report.BuildHtmlTableRows(
            resultList, displayList,
            actionlistdetailList, objParameters,
            listDisplayService
        );


        //khai bao success
        ViewData["success"] = "Thành công";

        return View();
      }
      catch (PostgresException ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching booking service info.");
        // Optionally, return an error view
        return View("~/Views/Pages/MiscError.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, exception = ex });
      }
    }

    // GET: NETReport/Editor_Utility
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // Tắt cache mặc định cho action này nếu cần thiết
    public async Task<IActionResult> Editor_Utility(string? ReportCode, int? id)
    {
      try
      {
        // neu không trả về report code thì chuyển sang link lỗi
        if (ReportCode == null)
        {
          return RedirectToAction("MiscError", "Pages", new { errorMessage = "Không tồn tại mã danh sách" });
        }
        ViewData["ReportCode"] = ReportCode;

        // Lấy dynamic query parameters
        var parameters = Request.Query;

        // lay thong tin report, va danh sach filter display cua report de xu ly
        var report = await _report.NET_Report_Get(ReportCode);
        // tra ve page loi neu khong tim thay report
        if (report == null)
        {
          return RedirectToAction("MiscError", "Pages", new { errorMessage = "Không tìm thấy bảng" });
        }
        // chuyen cau hinh report len giao dien de xu ly
        ViewData["report"] = report;

        string? connectionString = null;
        //neu datasourceId la null thi lay connectionString mac dinh
        if (report.ContainsKey("datasourceid"))
        {
          if (report["datasourceid"] != null)
          {
            //lay connectionstring tu report de goi store
            connectionString = await _datasrc.GetConnectionString(Convert.ToInt32(report["datasourceid"]));
          }
        }

        // khai bao cac du lieu report can su dung trong controller
        string? sqlContent = report.ContainsKey("sqlcontent") ? Convert.ToString(report["sqlcontent"]) : "";
        string? sqlDefaultContent = report.ContainsKey("sqldefaultcontent") ? Convert.ToString(report["sqldefaultcontent"]) : "";
        string? storeDRDisplay = report.ContainsKey("storedrdisplay") ? Convert.ToString(report["storedrdisplay"]) : "";

        if (string.IsNullOrWhiteSpace(sqlContent))
        {
          return RedirectToAction("MiscError", "Pages", new { errorMessage = "Không tồn tại store của bảng" });
        }

        // chuyen parameters cua bo loc thanh Idictionary<string, object>
        Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value.ToString());

        // neu bo loc khong co va co store default filter thi lay du lieu mac dinh tu store
        if (objParameters.Count() == 0 && sqlDefaultContent != null && sqlDefaultContent != "")
        {
          var defaultFilter = await _report.NET_Report_GetDefaultFilter(null, sqlDefaultContent, connectionString);
          objParameters = defaultFilter != null ? new Dictionary<string, object>(defaultFilter) : new Dictionary<string, object>();
        }

        // nếu tồn tại id thì add id vao parameter
        if (id != null)
        {
          // nếu obj param đã có Id thì bỏ qua
          if (!objParameters.ContainsKey("id"))
          {
            objParameters.Add("id", id ?? (object)DBNull.Value);
          }
        }

        // chuyen bo loc len giao dien
        ViewData["ListFilterValue"] = objParameters;

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
        ViewData["displayList"] = displayList;
        ViewData["displayParentLevelNum"] = displayParentLevelNum;

        // xu ly bo loc filter
        ViewData["ListFilterConfig"] = filterList;
        // doi voi cac filter co kieu select (select box, dropdownbox, tagbox,...), day cac bo select vao SelectListItem va đóng gói trong Dictionary để xử lý trên giao diện
        // Tạo Dictionary chứa SelectList cho từng dropdown (theo DynamicFieldName)
        var listFilterService = new Dictionary<string, List<SelectListItem>>();

        //Chuyển kết quả sang Dictionary<string, List<SelectListItem>>
        listFilterService = await _netService.NET_Service_GetListSelectListByFilter(filterList, objParameters);
        //  Gán danh sach select cho cac filter vào ViewBag
        ViewData["DynamicServiceSelectOptions"] = listFilterService;


        // doi voi cac display co kieu select (select box, dropdownbox, tagbox,...), day cac bo select vao SelectListItem va đóng gói trong Dictionary để xử lý trên giao diện
        // Tạo Dictionary chứa SelectList cho từng dropdown (theo DynamicFieldName)
        var listDisplayService = new Dictionary<string, List<SelectListItem>>();

        //Chuyển kết quả sang Dictionary<string, List<SelectListItem>>
        listDisplayService = await _netService.NET_Service_GetListSelectListByDisplay(displayList, objParameters);
        //  Gán danh sach select cho cac display vào ViewBag
        ViewData["EditorDynamicServiceSelectOptions"] = listDisplayService;

        // Nhận chuỗi json validate cho validation editor
        var getValidationFromStore = await _report.NET_Report_GetValidation(ReportCode);
        var validationJson = getValidationFromStore.ContainsKey("value") ? Convert.ToString(getValidationFromStore["value"]) ?? "" : "";
        ViewData["editorcolumnvalidation"] = validationJson;

        // search
        stopwatch.Restart();
        var resultList = await _report.Report_search(objParameters, sqlContent, connectionString);
        stopwatch.Stop();
        _logger.LogInformation($"Query resultList executed in {stopwatch.ElapsedMilliseconds} ms");
        ViewData["resultList"] = resultList;

        // neu co loi tu action POST tra ve thi bao loi
        if (TempData["ErrorMessage"] != null)
        {
          ViewData["ErrorMessage"] = TempData["ErrorMessage"];
          TempData.Remove("ErrorMessage");
        }
        else
        {
          //khai bao success
          ViewData["success"] = "Thành công";
        }

        return View();
      }
      catch (PostgresException ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching booking service info.");
        // Optionally, return an error view
        return View("~/Views/Pages/MiscError.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, exception = ex });
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
        if (report.ContainsKey("datasourceid"))
        {
          if (report["datasourceid"] != null)
          {
            //lay connectionstring tu report de goi store
            connectionString = await _datasrc.GetConnectionString(Convert.ToInt32(report["datasourceid"]));
          }
        }

        // khai bao cac du lieu report can su dung trong controller
        string? sqlEditContent = report.ContainsKey("sqleditcontent") ? Convert.ToString(report["sqleditcontent"]) : "";

        if (string.IsNullOrWhiteSpace(sqlEditContent))
        {
          TempData["ErrorMessage"] = "Không tồn tại store cập nhật dữ liệu của bảng";
          return Redirect($"{currentPath}?{queryString}");
        }

        bool ai_isactive = report.ContainsKey("ai_isactive") ? Convert.ToBoolean(report["ai_isactive"] ?? "false") : false;

        if (ai_isactive)
        {
          string ai_provider = report.ContainsKey("ai_provider") ? Convert.ToString(report["ai_provider"]) : "";
          string ai_model = report.ContainsKey("ai_model") ? Convert.ToString(report["ai_model"]) : "";
          string ai_storeGetSystemPrompt = report.ContainsKey("ai_storegetsystemprompt") ? Convert.ToString(report["ai_storegetsystemprompt"]) : "";
          string ai_requestColumn = report.ContainsKey("ai_requestcolumn") ? Convert.ToString(report["ai_requestcolumn"]) : "";

          string ai_systemPrompt = await _re.GetSystemPrompt(ai_storeGetSystemPrompt, null);
          form = await _re.ProcessFormWithAIAsync(form, "content,quantity", ai_systemPrompt, ai_provider, ai_model);
        }

        ///////////////////////////////////////// xử lý lưu editor ////////////////////////////
        //// Convert the IFormCollection to a dictionary of strings
        //var formData = form.ToDictionary(
        //                pair => pair.Key,
        //                pair => (object)pair.Value.ToString()  // Ensure each value is a string (flatten StringValues)
        //            );

        //string provider = "openrouter";
        //string model = "deepseek/deepseek-chat";
        //string systemPrompt = """
        //    Bạn là bộ phân loại chi phí cho phần mềm quản lý chi tiêu.
        //    Với tên chi phí do người dùng nhập, hãy trả về DUY NHẤT một JSON object,
        //    KHÔNG kèm markdown, KHÔNG kèm text giải thích, đúng format:
        //    {"category": string, "faIcon": string, "colorHex": string}

        //    Quy định:
        //    - faIcon phải là icon có thật của FontAwesome Free 6.
        //    - Không tự tạo icon.
        //    - Ưu tiên icon trực quan nhất.

        //      Ví dụ:

        //      fa-bolt
        //      fa-lightbulb
        //      fa-house
        //      fa-bed
        //      fa-gas-pump
        //      fa-car
        //      fa-faucet
        //      fa-utensils
        //      fa-shirt
        //      fa-book
        //      fa-laptop
        //      fa-server
        //      fa-wifi
        //      fa-coins
        //      ...
        //    - colorHex là mã màu hex phù hợp tâm lý màu theo nhóm chi phí
        //    - category là tên nhóm chi phí ngắn gọn bằng tiếng Việt
        //    """;
        //string request = "Tiền điện tháng 5";
        //var airesponse = await _re.AIResponse(provider, model, systemPrompt, request);

        //form = await form.ProcessAIColumnsAsync(
        //    aiRequestColumns: "content,quantity",
        //    provider: "openrouter",
        //    model: "deepseek/deepseek-chat",
        //    systemPrompt: systemPrompt,
        //    aiFunc: AIResponse
        ////);

        //string provider = "openrouter";
        //string model = "deepseek/deepseek-chat";
        //string systemPrompt = """
        //      Bạn là bộ phân loại chi phí cho phần mềm quản lý chi tiêu.
        //      Dữ liệu đầu vào của bạn sẽ là một JSON Object chứa các thông tin chi tiết của chi phí (ví dụ: {"content": "...", "quantity": "..."}). Hãy dựa vào trường "content" (và các thông tin bổ trợ khác nếu có) để phân loại chính xác.

        //      Yêu cầu kết quả trả về:
        //      Trả về DUY NHẤT một JSON object hợp lệ, đúng format sau:
        //      {"category": string, "faIcon": string, "colorHex": string}

        //      Quy định:
        //      - faIcon phải là icon có thật của FontAwesome Free 6. Không tự tạo icon. Ưu tiên icon trực quan nhất (Ví dụ: fa-bolt, fa-lightbulb, fa-house, fa-car, fa-utensils, fa-wifi, fa-coins...).
        //      - colorHex là mã màu hex phù hợp tâm lý màu theo nhóm chi phí.
        //      - category là tên nhóm chi phí ngắn gọn bằng tiếng Việt.

        //      CRITICAL WARNING: 
        //      - KHÔNG bọc kết quả trong các thẻ markdown code block như ```json ... ``` hoặc ``` ... ```.
        //      - KHÔNG kèm bất kỳ text giải thích nào khác. 
        //      - Chỉ trả về chuỗi JSON thuần túy bắt đầu bằng { và kết thúc bằng }.
        //      """;
        ////string request = "Tiền điện tháng 5";
        //form = await _re.ProcessFormWithAIAsync(form, "content,quantity" , systemPrompt, provider, model);

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
          return Redirect($"{currentPath}?{queryString}");
        }
      }
      catch (PostgresException ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching booking service info.");
        // Optionally, return an error view
        return View("~/Views/Pages/MiscError.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, exception = ex });
      }
    }

    // danh sach editor trong form
    [HttpGet]
    public async Task<IActionResult> Form_Report_Editor(string ReportCode, int? id, string? containerId, bool isPage = false)
    {
      try
      {
        // truyền isPage qua view để kiểm tra hiển thị (nếu các component nhỏ như popup hoặc report trong form thì giới hạn hiển thị)
        ViewData["isPage"] = isPage;
        // giữ lại containerId của thao tác trước đó để xử lý bộ lọc
        ViewData["containerId"] = containerId;

        // neu không trả về report code thì chuyển sang link lỗi
        if (ReportCode == null)
        {
          return Json(new { success = false, errorMessage = "Không tồn tại mã danh sách" });
        }
        ViewData["ReportCode"] = ReportCode;

        // Lấy dynamic query parameters
        var parameters = Request.Query;

        // lay thong tin report, va danh sach filter display cua report de xu ly
        var report = await _report.NET_Report_Get(ReportCode);
        // tra ve page loi neu khong tim thay report
        if (report == null)
        {
          return Json(new { success = false, errorMessage = "Không tìm thấy bảng" });
        }
        // chuyen cau hinh report len giao dien de xu ly
        ViewData["report"] = report;

        string? connectionString = null;
        //neu datasourceId la null thi lay connectionString mac dinh
        if (report.ContainsKey("datasourceid"))
        {
          if (report["datasourceid"] != null)
          {
            //lay connectionstring tu report de goi store
            connectionString = await _datasrc.GetConnectionString(Convert.ToInt32(report["datasourceid"]));
          }
        }

        // khai bao cac du lieu report can su dung trong controller
        string? sqlContent = report.ContainsKey("sqlcontent") ? Convert.ToString(report["sqlcontent"]) : "";
        string? sqlDefaultContent = report.ContainsKey("sqldefaultcontent") ? Convert.ToString(report["sqldefaultcontent"]) : "";
        string? storeDRDisplay = report.ContainsKey("storedrdisplay") ? Convert.ToString(report["storedrdisplay"]) : "";

        if (string.IsNullOrWhiteSpace(sqlContent))
        {
          return Json(new { success = false, errorMessage = "Không tồn tại store của bảng" });
        }

        // chuyen parameters cua bo loc thanh Idictionary<string, object>
        Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value.ToString());

        // neu bo loc khong co va co store default filter thi lay du lieu mac dinh tu store
        if (objParameters.Count() == 0 && sqlDefaultContent != null && sqlDefaultContent != "")
        {
          var defaultFilter = await _report.NET_Report_GetDefaultFilter(null, sqlDefaultContent, connectionString);
          objParameters = defaultFilter != null ? new Dictionary<string, object>(defaultFilter) : new Dictionary<string, object>();
        }

        // nếu tồn tại id thì add id vao parameter
        if (id != null)
        {
          // nếu obj param đã có Id thì bỏ qua
          if (!objParameters.ContainsKey("id"))
          {
            objParameters.Add("id", id ?? (object)DBNull.Value);
          }
        }

        // chuyen bo loc len giao dien
        ViewData["ListFilterValue"] = objParameters;

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
        ViewData["displayList"] = displayList;
        ViewData["displayParentLevelNum"] = displayParentLevelNum;

        // xu ly bo loc filter
        ViewData["ListFilterConfig"] = filterList;
        // doi voi cac filter co kieu select (select box, dropdownbox, tagbox,...), day cac bo select vao SelectListItem va đóng gói trong Dictionary để xử lý trên giao diện
        // Tạo Dictionary chứa SelectList cho từng dropdown (theo DynamicFieldName)
        var listFilterService = new Dictionary<string, List<SelectListItem>>();

        //Chuyển kết quả sang Dictionary<string, List<SelectListItem>>
        listFilterService = await _netService.NET_Service_GetListSelectListByFilter(filterList, objParameters);
        //  Gán danh sach select cho cac filter vào ViewBag
        ViewData["DynamicServiceSelectOptions"] = listFilterService;


        // doi voi cac display co kieu select (select box, dropdownbox, tagbox,...), day cac bo select vao SelectListItem va đóng gói trong Dictionary để xử lý trên giao diện
        // Tạo Dictionary chứa SelectList cho từng dropdown (theo DynamicFieldName)
        var listDisplayService = new Dictionary<string, List<SelectListItem>>();

        //Chuyển kết quả sang Dictionary<string, List<SelectListItem>>
        listDisplayService = await _netService.NET_Service_GetListSelectListByDisplay(displayList, objParameters);
        //  Gán danh sach select cho cac display vào ViewBag
        ViewData["EditorDynamicServiceSelectOptions"] = listDisplayService;

        // search
        stopwatch.Restart();
        var resultList = await _report.Report_search(objParameters, sqlContent, connectionString);
        stopwatch.Stop();
        _logger.LogInformation($"Query resultList executed in {stopwatch.ElapsedMilliseconds} ms");
        ViewData["resultList"] = resultList;

        // neu co loi tu action POST tra ve thi bao loi
        if (TempData["ErrorMessage"] != null)
        {
          ViewData["ErrorMessage"] = TempData["ErrorMessage"];
          TempData.Remove("ErrorMessage");
          return Json(new { success = false, errorMessage = ViewData["ErrorMessage"] });
        }
        else
        {
          //khai bao success
          ViewData["success"] = "Thành công";
        }

        return PartialView("~/Views/Shared/Partial/MainPageLayout/_Form_Report_Editor_Partial.cshtml");
      }
      catch (PostgresException ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching booking service info.");
        // Optionally, return an error view
        return View("~/Views/Pages/MiscError.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, exception = ex });
      }

    }

    // POST: /report/editor-utility/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    public async Task<IActionResult> Form_Report_Editor(string? ReportCode, int? id, [FromForm] IFormCollection form)
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
          return Json(new { success = false, errorMessage = "Không tìm thấy bảng" });
        }

        string? connectionString = null;
        //neu datasourceId la null thi lay connectionString mac dinh
        if (report.ContainsKey("datasourceid"))
        {
          if (report["datasourceid"] != null)
          {
            //lay connectionstring tu report de goi store
            connectionString = await _datasrc.GetConnectionString(Convert.ToInt32(report["datasourceid"]));
          }
        }

        // khai bao cac du lieu report can su dung trong controller
        string? sqlEditContent = report.ContainsKey("sqleditcontent") ? Convert.ToString(report["sqleditcontent"]) : "";

        if (string.IsNullOrWhiteSpace(sqlEditContent))
        {
          return Json(new { success = false, errorMessage = "Không tồn tại store cập nhật dữ liệu của bảng" });
        }

        /////////////////////////////////////// xử lý lưu editor ////////////////////////////
        // Convert the IFormCollection to a dictionary of strings
        var formData = form.ToDictionary(
                        pair => pair.Key,
                        pair => (object)pair.Value.ToString()  // Ensure each value is a string (flatten StringValues)
                    );

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
          return Json(new { success = false, errorMessage = errorMessage });
        }
        // khong tra ve Id, cung khong tra ve error message thi bao loi chua tra ve id
        else
        {
          return Json(new { success = true });
        }
      }
      catch (PostgresException ex)
      {
        return Json(new { success = false, errorMessage = ex.Message });
      }
    }

    // danh sach editor trong form
    [HttpGet]
    public async Task<IActionResult> Form_Report_Viewer(string ReportCode, int? id, string? containerId, bool isPage = false)
    {
      try
      {
        // truyền isPage qua view để kiểm tra hiển thị (nếu các component nhỏ như popup hoặc report trong form thì giới hạn hiển thị)
        ViewData["isPage"] = isPage;
        // giữ lại containerId của thao tác trước đó để xử lý bộ lọc
        ViewData["containerId"] = containerId;
        // neu không trả về report code thì chuyển sang link lỗi
        if (ReportCode == null)
        {
          return Json(new { success = false, errorMessage = "Không tồn tại mã danh sách" });
        }
        ViewData["ReportCode"] = ReportCode;

        // Lấy dynamic query parameters
        var parameters = Request.Query;

        // lay thong tin report, va danh sach filter display cua report de xu ly
        var report = await _report.NET_Report_Get(ReportCode);
        // tra ve page loi neu khong tim thay report
        if (report == null)
        {
          return Json(new { success = false, errorMessage = "Không tìm thấy bảng" });
        }
        // chuyen cau hinh report len giao dien de xu ly
        ViewData["report"] = report;

        string? connectionString = null;
        //neu datasourceId la null thi lay connectionString mac dinh
        if (report.ContainsKey("datasourceid"))
        {
          if (report["datasourceid"] != null)
          {
            //lay connectionstring tu report de goi store
            connectionString = await _datasrc.GetConnectionString(Convert.ToInt32(report["datasourceid"]));
          }
        }

        // khai bao cac du lieu report can su dung trong controller
        string? sqlContent = report.ContainsKey("sqlcontent") ? Convert.ToString(report["sqlcontent"]) : "";
        string? sqlDefaultContent = report.ContainsKey("sqldefaultcontent") ? Convert.ToString(report["sqldefaultcontent"]) : "";
        string? storeDRDisplay = report.ContainsKey("storedrdisplay") ? Convert.ToString(report["storedrdisplay"]) : "";

        if (string.IsNullOrWhiteSpace(sqlContent))
        {
          return Json(new { success = false, errorMessage = "Không tồn tại store của bảng" });
        }

        // chuyen parameters cua bo loc thanh Idictionary<string, object>
        //Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)(kvp.Value.Count > 1 ? kvp.Value.Split(',', StringSplitOptions.RemoveEmptyEntries) : kvp.Value.ToString()));

        // With this corrected version:  
        Dictionary<string, object> objParameters = parameters.ToDictionary(
            kvp => kvp.Key,
            kvp => (object)(kvp.Value.ToString())
        );
        // neu bo loc khong co va co store default filter thi lay du lieu mac dinh tu store
        if (objParameters.Count() == 0 && sqlDefaultContent != null && sqlDefaultContent != "")
        {
          var defaultFilter = await _report.NET_Report_GetDefaultFilter(null, sqlDefaultContent, connectionString);
          objParameters = defaultFilter != null ? new Dictionary<string, object>(defaultFilter) : new Dictionary<string, object>();
        }

        // chuyen bo loc len giao dien
        ViewData["ListFilterValue"] = objParameters;

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
        ViewData["displayList"] = displayList;
        ViewData["displayParentLevelNum"] = displayParentLevelNum;

        // xu ly bo loc filter
        ViewData["ListFilterConfig"] = filterList;
        // doi voi cac filter co kieu select (select box, dropdownbox, tagbox,...), day cac bo select vao SelectListItem va đóng gói trong Dictionary để xử lý trên giao diện
        // Tạo Dictionary chứa SelectList cho từng dropdown (theo DynamicFieldName)
        var listFilterService = new Dictionary<string, List<SelectListItem>>();

        //Chuyển kết quả sang Dictionary<string, List<SelectListItem>>
        listFilterService = await _netService.NET_Service_GetListSelectListByFilter(filterList, objParameters);
        //  Gán danh sach select cho cac filter vào ViewBag
        ViewData["DynamicServiceSelectOptions"] = listFilterService;

        // doi voi cac display co kieu select (select box, dropdownbox, tagbox,...), day cac bo select vao SelectListItem va đóng gói trong Dictionary để xử lý trên giao diện
        // Tạo Dictionary chứa SelectList cho từng dropdown (theo DynamicFieldName)
        var listDisplayService = new Dictionary<string, List<SelectListItem>>();

        //Chuyển kết quả sang Dictionary<string, List<SelectListItem>>
        listDisplayService = await _netService.NET_Service_GetListSelectListByDisplay(displayList, objParameters);
        //  Gán danh sach select cho cac display vào ViewBag
        ViewData["EditorDynamicServiceSelectOptions"] = listDisplayService;

        // lấy danh sách action list detail theo object code là report code
        var actionlistdetailList = await _action.NET_ActionListDetail_WithObject_Get(ReportCode, null, "REPORT");
        // chuyển action list detail lên giao diện để xử lý
        ViewData["actionlistdetailList"] = actionlistdetailList;

        // search
        stopwatch.Restart();
        var resultList = await _report.Report_search(objParameters, sqlContent, connectionString);
        stopwatch.Stop();
        _logger.LogInformation($"Query resultList executed in {stopwatch.ElapsedMilliseconds} ms");
        ViewData["resultList"] = resultList;

        //khai bao success
        ViewData["success"] = "Thành công";

        return PartialView("~/Views/Shared/Partial/MainPageLayout/_Form_Report_Viewer_Partial.cshtml");
      }
      catch (PostgresException ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching booking service info.");
        // Optionally, return an error view
        return View("~/Views/Pages/MiscError.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, exception = ex });
      }

    }

    [Authorize]
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> ReportBuilder(string ReportCode = "F0_HS_Booking1", bool CreateNew = false)
    {
      if (string.IsNullOrWhiteSpace(ReportCode))
      {
        ViewData["ErrorMessage"] = "Không tồn tại mã báo cáo";
        return View();
      }

      ViewData["ReportCode"] = ReportCode.Trim();
      ViewData["CreateNew"] = CreateNew;

      var dynamicFields = await _report.NET_DynamicField_Search();
      ViewData["ReportBuilderDynamicFields"] = JsonSerializer.Serialize(dynamicFields.Select(item =>
      {
        var row = (IDictionary<string, object>)item;
        object Value(string key) => row.TryGetValue(key, out var value) ? value : null;
        string name = Convert.ToString(Value("name")) ?? "";
        string type = Convert.ToString(Value("type")) ?? "";
        return new
        {
          value = Value("id"),
          label = string.IsNullOrWhiteSpace(type) ? name : $"{name} ({type})"
        };
      }));

      if (!CreateNew)
      {
        var report = await _report.NET_Report_Get(ReportCode.Trim());
        if (report == null)
        {
          ViewData["ErrorMessage"] = "Không tìm thấy báo cáo " + ReportCode.Trim();
          return View();
        }

        var reportValues = (IDictionary<string, object>)report;
        object GetValue(string key) => reportValues.TryGetValue(key, out var value) ? value : null;
        bool GetBool(string key, bool defaultValue = false) => GetValue(key) == null ? defaultValue : Convert.ToBoolean(GetValue(key));
        string GetText(string key, string defaultValue = "") => Convert.ToString(GetValue(key)) ?? defaultValue;

        var filterTask = _report.NET_Filter_WithReport_Get(ReportCode.Trim(), null);
        var displayTask = _report.NET_Display_WithReport_Get(new Dictionary<string, object>(), ReportCode.Trim(), null);
        var actionTask = _action.NET_ActionListDetail_WithObject_Get(ReportCode.Trim(), null, "REPORT");
        await Task.WhenAll(filterTask, displayTask, actionTask);

        var displays = await displayTask;
        var filters = await filterTask;
        var actions = await actionTask;
        var groups = displays.Where(item => Convert.ToBoolean(((IDictionary<string, object>)item).TryGetValue("isparent", out var value) && value != null ? value : false))
          .Select(item =>
          {
            var row = (IDictionary<string, object>)item;
            string Text(string key) => row.TryGetValue(key, out var value) ? Convert.ToString(value) ?? "" : "";
            return new { id = Text("code").ToLowerInvariant(), title = Text("name"), cssheader = Text("cssheader"), databaseId = row.TryGetValue("id", out var id) ? id : null };
          }).ToList();

        string Renderer(string type)
        {
          type = (type ?? "").ToLowerInvariant();
          return type switch
          {
            "float" or "int" or "long" => "number",
            "date" => "date",
            "datetime" => "datetime",
            "icons" => "icons",
            "file" => "link",
            "combobox" => "badge",
            "textarea" => "text",
            _ => type == "link" ? "html" : "text"
          };
        }

        string DataType(string type)
        {
          type = (type ?? "").ToLowerInvariant();
          return type switch
          {
            "float" or "int" or "long" => "number",
            "date" => "date",
            "datetime" => "datetime",
            "combobox" => "select",
            _ => "text"
          };
        }

        var columns = displays.Where(item =>
        {
          var row = (IDictionary<string, object>)item;
          return !(row.TryGetValue("isparent", out var value) && value != null && Convert.ToBoolean(value));
        }).Select(item =>
        {
          var row = (IDictionary<string, object>)item;
          object Value(string key) => row.TryGetValue(key, out var value) ? value : null;
          string Text(string key) => Convert.ToString(Value(key)) ?? "";
          bool Bool(string key, bool fallback = false) => Value(key) == null ? fallback : Convert.ToBoolean(Value(key));
          int width = int.TryParse(Text("width"), out var parsedWidth) ? Math.Clamp(parsedWidth, 60, 800) : 150;
          string sourceType = Text("type");
          string align = Text("textalign").ToLowerInvariant();
          if (align != "center" && align != "right") align = "left";
          return new
          {
            id = "display_" + Convert.ToString(Value("id")), databaseId = Value("id"), key = Text("code").ToLowerInvariant(), title = Text("name"),
            type = DataType(sourceType), renderer = Renderer(sourceType), sourceType, format = Text("format"), width, align,
            @fixed = Bool("isfreepane") ? "left" : "", visible = Bool("isdisplay", true), sortable = Bool("issort", true),
            filterable = true, isexport = Bool("isexport", true), groupId = Text("parentcode").ToLowerInvariant(), mobileRole = Bool("isdisplay", true) ? "summary" : "hidden",
            cssheader = Text("cssheader"), csscell = "", iconClass = "", template = "", serviceId = Value("serviceid"),
            aggregate = Bool("issum") ? "sum" : "", isreadonly = Bool("isreadonly"), colnum = Value("colnum")
          };
        }).ToList();

        string FilterComponent(string dynamicFieldName)
        {
          dynamicFieldName = (dynamicFieldName ?? "").ToUpperInvariant();
          if (dynamicFieldName.Contains("DATE") && dynamicFieldName.Contains("RANGE")) return "dateRange";
          if (dynamicFieldName.Contains("DATE")) return "date";
          if (dynamicFieldName.Contains("TREEVIEW") || dynamicFieldName.Contains("MULTIPLE")) return "multiSelect";
          if (dynamicFieldName.Contains("SELECT") || dynamicFieldName.Contains("DROP")) return "select";
          return "text";
        }

        var filterConfig = filters.Select(item =>
        {
          var row = (IDictionary<string, object>)item;
          object Value(string key) => row.TryGetValue(key, out var value) ? value : null;
          string Text(string key) => Convert.ToString(Value(key)) ?? "";
          bool Bool(string key, bool fallback = false) => Value(key) == null ? fallback : Convert.ToBoolean(Value(key));
          string component = FilterComponent(Text("dynamicfieldname"));
          return new
          {
            id = "filter_" + Convert.ToString(Value("id")), databaseId = Value("id"), field = Text("code").ToLowerInvariant(), label = Text("name"), component,
            @operator = component == "dateRange" ? "between" : component == "multiSelect" ? "in" : component == "text" ? "contains" : "equals",
            colSpan = Math.Clamp(Value("colspan") == null ? 4 : Convert.ToInt32(Value("colspan")), 1, 12), options = Array.Empty<object>(),
            dynamicFieldId = Value("dynamicfieldid"), serviceId = Value("seviceid"), required = Bool("required"), enabled = Bool("isactive", true), orderId = Value("orderid")
          };
        }).ToList();

        var actionConfig = actions.Select(item =>
        {
          var row = (IDictionary<string, object>)item;
          object Value(string key) => row.TryGetValue(key, out var value) ? value : null;
          string Text(string key) => Convert.ToString(Value(key)) ?? "";
          bool Bool(string key, bool fallback = false) => Value(key) == null ? fallback : Convert.ToBoolean(Value(key));
          string css = Text("cssbutton");
          string background = "#7759ed";
          string color = "#ffffff";
          try
          {
            if (!string.IsNullOrWhiteSpace(css))
            {
              using var cssJson = JsonDocument.Parse(css);
              if (cssJson.RootElement.TryGetProperty("background", out var bg) && !string.IsNullOrWhiteSpace(bg.GetString())) background = bg.GetString();
              if (cssJson.RootElement.TryGetProperty("color", out var fg) && !string.IsNullOrWhiteSpace(fg.GetString())) color = fg.GetString();
            }
          }
          catch (JsonException) { }
          return new
          {
            id = "actiondetail_" + Convert.ToString(Value("actionlistdetailid")), detailId = Value("actionlistdetailid"), actionId = Value("actionid"),
            name = Text("actionname"), code = Text("actioncode"), scope = Bool("istop") ? "top" : "row", type = Text("type").ToUpperInvariant(),
            icon = Text("actionicon"), iconStyle = "", background, color, value = Text("value"), enabled = Bool("isactive", true),
            requiresSelection = Bool("ischoosedata"), confirm = Bool("ispopupconfirm"), orderId = Value("actionlistdetailorderid"), dataSourceId = Value("datasourceid")
          };
        }).ToList();

        var previewRows = new List<dynamic>();
        try
        {
          string connectionString = GetValue("datasourceid") == null
            ? null
            : await _datasrc.GetConnectionString(Convert.ToInt32(GetValue("datasourceid")));
          var previewParameters = new Dictionary<string, object>();
          string defaultStore = GetText("sqldefaultcontent");
          if (!string.IsNullOrWhiteSpace(defaultStore))
          {
            var defaultValues = await _report.NET_Report_GetDefaultFilter(null, defaultStore, connectionString);
            if (defaultValues != null) previewParameters = new Dictionary<string, object>(defaultValues);
          }
          string dataStore = GetText("sqlcontent");
          if (!string.IsNullOrWhiteSpace(dataStore))
          {
            previewRows = (await _report.Report_search(previewParameters, dataStore, connectionString)).Take(100).ToList();
          }
        }
        catch (Exception ex) when (ex is SqlException || ex is PostgresException || ex is InvalidOperationException)
        {
          _logger.LogWarning(ex, "Không tải được dữ liệu preview cho report {ReportCode}", ReportCode);
          ViewData["ReportBuilderWarning"] = "Đã tải cấu hình nhưng chưa tải được dữ liệu preview.";
        }

        var initialConfig = new
        {
          version = "3.0.0",
          databaseVersion = GetValue("lastmodificationtime"),
          table = new
          {
            id = GetText("code"), title = GetText("name", "Danh sách"), subtitle = "Cấu hình từ TTT_Config", rowKey = "id",
            pagination = GetBool("pagination", true), pageSize = 20, striped = true, rowSelection = !string.Equals(GetText("selectiontype"), "none", StringComparison.OrdinalIgnoreCase),
            rowDetails = true, showRowActions = true, showSummary = true, datasourceId = GetValue("datasourceid"), sqlContent = GetText("sqlcontent"),
            sqlDefaultContent = GetText("sqldefaultcontent"), sqlEditContent = GetText("sqleditcontent"), showToolbar = GetBool("showtoolbar", true),
            showSearchBar = GetBool("issearchbar", true), exportExcel = GetBool("isexportexcel", true), createNew = false
          },
          display = new { theme = "light-dashboard", density = "comfortable", accent = "#8355f4", showHeader = true, showFilterBar = !GetBool("disablesearch"), allowColumnToggle = true, responsive = true, mobileMode = "cards", filterOpen = false, titleStyle = "", headerStyle = "", description = GetText("description"), mobileActionLabels = true },
          groups, columns, filters = filterConfig, actions = actionConfig, rowRules = Array.Empty<object>(), data = new { mode = "database", rows = previewRows }
        };
        ViewData["ReportBuilderInitialConfig"] = JsonSerializer.Serialize(initialConfig);
      }
      return View();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReportBuilderSave(string ReportCode, [FromBody] JsonElement config)
    {
      if (string.IsNullOrWhiteSpace(ReportCode) || config.ValueKind != JsonValueKind.Object)
      {
        return BadRequest(new { success = false, errorMessage = "Cấu hình hoặc mã báo cáo không hợp lệ." });
      }

      long? userId = long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var parsedUserId) ? parsedUserId : null;
      var result = await _report.NET_ReportBuilder_Save(ReportCode.Trim(), config.GetRawText(), userId);
      if (result == null)
      {
        return StatusCode(500, new { success = false, errorMessage = "Store lưu cấu hình không trả kết quả." });
      }

      bool success = result.TryGetValue("success", out var successValue) && Convert.ToBoolean(successValue);
      string errorMessage = result.TryGetValue("errormessage", out var errorValue) ? Convert.ToString(errorValue) : null;
      return Json(new { success, errorMessage, result });
    }
  }
}
