using AspnetCoreMvcFull.Models;
using KOAHome.EntityFramework;
using KOAHome.Helpers;
using KOAHome.Models;
using KOAHome.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace KOAHome.Controllers
{
  public class NETFormController : Controller
  {
    private readonly ILogger<NETFormController> _logger;
    private readonly QLKCL_NEWContext _db;
    private readonly IReportEditorService _re;
    private readonly IAttachmentService _att;
    private readonly IReportService _report;
    private readonly IFormService _form;
    private readonly IFormBuilderService _formBuilder;
    private readonly IActionService _action;
    private readonly IWidgetService _widget;
    private readonly IDRDatasourceService _datasrc;
    private readonly INetServiceService _netService;
    private readonly IConnectionService _con;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _googleCloudVisionApiKey;

    public NETFormController(QLKCL_NEWContext db, ILogger<NETFormController> logger, IReportEditorService re, IAttachmentService att, IReportService report, IFormService form, IFormBuilderService formBuilder, IActionService action, IWidgetService widget, IDRDatasourceService datasrc, INetServiceService netService, IConnectionService con, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
      _db = db;
      _logger = logger;
      _re = re;
      _att = att;
      _report = report;
      _form = form;
      _formBuilder = formBuilder;
      _action = action;
      _widget = widget;
      _datasrc = datasrc;
      _netService = netService;
      _con = con;
      _httpClientFactory = httpClientFactory;
      _googleCloudVisionApiKey = configuration["Google:CloudVisionApiKey"];
    }

    // GET: NETFormController
    public ActionResult Index()
    {
      return View();
    }

    // GET: HsBookings/Edit/5
    public async Task<IActionResult> Viewer(string? FormCode, int? id, bool isReadOnly = false, bool isPage = false)
    {
      try
      {
        // truyền isPage qua view để kiểm tra hiển thị (nếu các component nhỏ như popup hoặc report trong form thì giới hạn hiển thị)
        ViewData["isPage"] = isPage;

        // mac dinh id = 0
        id ??= 0;

        ViewData["id"] = id;

        // neu không trả về report code thì chuyển sang link lỗi
        if (FormCode == null)
        {
          ViewData["ErrorMessage"] = "Không tồn tại mã biểu mẫu";
          return View();
        }
        ViewData["FormCode"] = FormCode;

        // Lấy dynamic query parameters
        var parameters = Request.Query;

        // lay thong tin report, va danh sach filter display cua report de xu ly
        var config_form = await _form.NET_Form_Get(FormCode);
        // tra ve page loi neu khong tim thay report
        if (config_form == null)
        {
          ViewData["ErrorMessage"] = "Không tìm thấy biểu mẫu";
          return View();
        }
        // chuyen cau hinh form len giao dien de xu ly
        ViewData["config_form"] = config_form;

        string? connectionString = null;
        //neu datasourceId la null thi lay connectionString mac dinh
        if (config_form.ContainsKey("datasourceid"))
        {
          if (config_form["datasourceid"] != null)
          {
            //lay connectionstring tu cau hinh form de goi store
            connectionString = await _datasrc.GetConnectionString(Convert.ToInt32(config_form["datasourceid"]));
          }
        }

        // khai bao cac du lieu cau hinh form can su dung trong controller
        string? storeDefaultData = config_form.ContainsKey("storedefaultdata") ? Convert.ToString(config_form["storedefaultdata"]) : "";
        string? storeGetData = config_form.ContainsKey("storegetdata") ? Convert.ToString(config_form["storegetdata"]) : "";
        //string? storeSetData = config_form.ContainsKey("StoreSetData") ? Convert.ToString(config_form["StoreSetData"]) : "";

        if (string.IsNullOrWhiteSpace(storeDefaultData) && string.IsNullOrWhiteSpace(storeGetData) == null)
        {
          ViewData["ErrorMessage"] = "Không tồn tại store lây dữ liệu biểu mẫu";
          return View();
        }

        // chuyen parameters cua duong dan thanh Idictionary<string, object>
        Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value.ToString());

        // lay danh sach dynamic field cua form de xu ly
        var stopwatch = Stopwatch.StartNew();
        var config_formfield = await _form.NET_Form_VersionField_WithForm_sel(FormCode, null);
        stopwatch.Stop();
        _logger.LogInformation($"Query config_formfieldTask executed in {stopwatch.ElapsedMilliseconds} ms");

        // chuyển cấu hình form field lên giao diện để xử lý
        ViewData["config_formfield"] = config_formfield;

        // doi voi cac fiekd co kieu select (select box, dropdownbox, tagbox,...), day cac bo select vao SelectListItem va đóng gói trong Dictionary để xử lý trên giao diện
        // Tạo Dictionary chứa SelectList cho từng dropdown (theo DynamicFieldName)
        var config_formfieldService = new Dictionary<string, List<SelectListItem>>();

        //Chuyển kết quả sang Dictionary<string, List<SelectListItem>>
        config_formfieldService = await _netService.NET_Service_GetListSelectListByFormField(config_formfield, objParameters);
        //  Gán danh sach select cho cac filter vào ViewBag
        ViewData["DynamicServiceSelectOptions"] = config_formfieldService;

        // Nhận chuỗi json validate cho validation form
        var validationJson = await _form.NET_Form_GetValidation(FormCode);
        ViewData["fieldvalidation"] = validationJson;

        //khai bao phan tu chua data
        var formData = await _form.Form_sel(objParameters, id, (id == 0 ? storeDefaultData : storeGetData), connectionString);
        ViewData["formData"] = formData;

        // xu ly file
        // Kiểm tra xem form có file nào không
        // lay danh sach object type code tu config form neu co field file uploader
        string attObjectTypeCodes = config_form.ContainsKey("attobjecttypecodes") ? Convert.ToString(config_form["attobjecttypecodes"]) : "";

        if (!string.IsNullOrEmpty(attObjectTypeCodes))
        {
          ViewData["fileUrls"] = await _att.HandleFiles(attObjectTypeCodes, null, id);
        }

        // danh sach service theo booking 
        var reportResultList = await _report.ReportDetail_FromParent("bookingid", (id ?? 0).ToString(), "HS_BookingService_search", null);
        ViewData["reportResultList"] = reportResultList;

        // set readonly form neu co isreadonly = false
        ViewData["IsReadOnly"] = isReadOnly;

        // neu co loi tu action POST tra ve thi bao loi
        if (TempData["ErrorMessage"] != null)
        {
          ViewData["ErrorMessage"] = TempData["ErrorMessage"];
          TempData.Remove("ErrorMessage");
          return View();
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
        _logger.LogError(ex, "An error occurred while fetching form.");
        // Optionally, return an error view
        return View("~/Views/Pages/MiscError.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, exception = ex });
      }

    }

    // POST: HsBookings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost("/form/viewer/{FormCode}/{id?}")]
    //[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Viewer(string? FormCode, int? id, [FromForm] IFormCollection form)
    {
      try
      {
          // reset tempdata error message
          TempData["ErrorMessage"] = null;

        // mac dinh id = 0
        id ??= 0;

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
        var config_form = await _form.NET_Form_Get(FormCode);
        // tra ve page loi neu khong tim thay report
        if (config_form == null)
        {
          TempData["ErrorMessage"] = "Không tìm thấy biểu mẫu";
          return Redirect($"{currentPath}?{queryString}");
        }

        string? connectionString = null;
        //neu datasourceId la null thi lay connectionString mac dinh
        if (config_form.ContainsKey("datasourceid"))
        {
          if (config_form["datasourceid"] != null)
          {
            //lay connectionstring tu cau hinh form de goi store
            connectionString = await _datasrc.GetConnectionString(Convert.ToInt32(config_form["datasourceid"]));
          }
        }

        // khai bao cac du lieu cau hinh form can su dung trong controller
        string? storeSetData = config_form.ContainsKey("storesetdata") ? Convert.ToString(config_form["storesetdata"]) : "";
        // kiểm tra kiểu lưu dữ liệu editor (1.Form trước editor sau, 2. Editor trước form sau, 3. Attachment trước -> Form -> Editor)
        // mặc định là 1
        int saveEditorType = config_form.ContainsKey("saveeditortype") ? Convert.ToInt32(config_form["saveeditortype"] ?? 1) : 1;

        if (string.IsNullOrWhiteSpace(storeSetData))
        {
          TempData["ErrorMessage"] = "Không tồn tại store xử lý dữ liệu biểu mẫu";
          return Redirect($"{currentPath}?{queryString}");
        }

        // xu ly file
        // Kiểm tra xem form có file nào không
        // lay danh sach object type code tu config form neu co field file uploader
        string attObjectTypeCodes = config_form.ContainsKey("attobjecttypecodes") ? Convert.ToString(config_form["attobjecttypecodes"]) : "";

        if (!string.IsNullOrEmpty(attObjectTypeCodes))
        {
          await _att.HandleFiles(attObjectTypeCodes, form, id);
        }

        // Convert the IFormCollection to a dictionary of strings
        var formData = form.ToDictionary(
                        pair => pair.Key,
                        pair => (object)pair.Value.ToString()  // Ensure each value is a string (flatten StringValues)
                    );


        // nếu saveEditorType là 3 (Lưu attachment trước form sau) thì lưu attachment ở đây và trả về list attachmentid cho store set data
        if (!string.IsNullOrEmpty(attObjectTypeCodes) && saveEditorType == 3)
        {
          // xu ly luu bang attachment
          var saveAttachmentResult = await _att.SaveAttachmentTable(form, id ?? 0);

          // Dùng JsonConvert để chuyển về JObject hoặc dynamic
          var json = JObject.FromObject(saveAttachmentResult); // nếu dùng Newtonsoft.Json
          bool success = json["success"]?.Value<bool>() ?? false;

          if (!success)
          {
            string error = json["errorMessage"]?.ToString();
            TempData["ErrorMessage"] = error ?? "Lưu file không thành công";
            return Redirect($"{currentPath}?{queryString}");
          }

          // Nếu thành công
          string listAttachmentId = json["listAttachmentId"]?.ToString(); // VD: "11233,11234"
          // đưa list attachment id vào formData để xử lý ở store lưu form
          formData["attachmentids"] = listAttachmentId;
        }

        //// gui form data len view de hien thi
        //ViewData["formData"] = formData;

        var resultList = await _form.Form_ups(formData, id, storeSetData, connectionString);
        //kiem tra du lieu id tra ve
        var id_return = resultList
        .Where(item => ((IDictionary<string, object>)item).ContainsKey("id"))
        .Select(item => ((IDictionary<string, object>)item)["id"])
        .FirstOrDefault(); // Lọc ra những phần tử có Id

        // neu co gia tri tra ve thi bao thanh cong
        if (id_return != null && int.TryParse(id_return.ToString(), out int num) && num > 0)
        {
          id = (int)id_return;

          // nếu saveEditorType là 3 (Lưu attachment trước form sau) thì không cần lưu attachment ở đây
          if (!string.IsNullOrEmpty(attObjectTypeCodes) && saveEditorType != 3)
          {
            // xu ly luu bang attachment
            var saveAttachmentResult = await _att.SaveAttachmentTable(form, id ?? 0);

            // Dùng JsonConvert để chuyển về JObject hoặc dynamic
            var json = JObject.FromObject(saveAttachmentResult); // nếu dùng Newtonsoft.Json
            bool success = json["success"]?.Value<bool>() ?? false;

            if (!success)
            {
              string error = json["errorMessage"]?.ToString();
              TempData["ErrorMessage"] = error ?? "Lưu file không thành công";
              return Redirect($"{currentPath}?{queryString}");
            }
          }

          //xu ly report form
          // lấy danh sách report code thuộc form
          string stringaggreportcodes = await _form.NET_Form_GetListReportCode(FormCode);
          // với mỗi report code đang có thì xử lý
          if (!string.IsNullOrWhiteSpace(stringaggreportcodes))
          {
            var reportCodes = stringaggreportcodes.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var code in reportCodes)
            {
              var reportCode = code.Trim();

              // lay thong tin report de xu ly
              var report = await _report.NET_Report_Get(reportCode);
              // tìm thấy report thì tiếp tục
              if (report != null)
              {
                // khai bao cac du lieu report can su dung trong controller
                string? sqlEditContent = report.ContainsKey("sqleditcontent") ? Convert.ToString(report["sqleditcontent"]) : "";
                // Dictionary để nhóm dữ liệu theo số thứ tự [n]
                // Chuyển đổi dữ liệu sang JSON (loc du lieu form tra ve lay du lieu grid va chuyen thanh json)
                string reportJsonData = await _re.ExtractGridDataToJson(form);
                //end xu ly report form
                var reportResultList = await _re.ReportEditor_Json_Update(queryParamerter, id, reportJsonData, sqlEditContent, null);
                //kiem tra ton tai error message
                // Kiểm tra và nối giá trị của ErrorMessage
                if (_con.CheckForErrors(reportResultList, out string errorMessage))
                {
                  TempData["ErrorMessage"] = errorMessage;
                  return Redirect($"{currentPath}?{queryString}");
                }
              }
            }
          }
          // khong tra ve Id, cung khong tra ve error message thi bao loi chua tra ve id
          return Redirect($"{currentPath}?{queryString}");

        }
        else
        {
          //kiem tra ton tai error message
          // Kiểm tra và nối giá trị của ErrorMessage
          if (_con.CheckForErrors(resultList, out string errorMessage))
          {
            TempData["ErrorMessage"] = errorMessage;
            return Redirect($"{currentPath}?{queryString}");
          }
          // khong tra ve Id, cung khong tra ve error message thi bao loi chua tra ve id
          else
          {
            TempData["ErrorMessage"] = "Chưa trả về Id";
            return Redirect($"{currentPath}?{queryString}");
          }
        }
      }
      catch (PostgresException ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching form.");
        // Optionally, return an error view
        return View("~/Views/Pages/MiscError.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, exception = ex });
      }
    }

    // popup form view component
    [HttpGet]
    public async Task<IActionResult> PopupForm(string FormCode, int? id, bool? isReadOnly = false, string? containerId = "", bool? isStepper = false)
    {
      try
      {
        // isreadonly: kiểm tra form chỉ đọc, containerid: id của container cha của form popup, isStepper: kiểm tra form có thuộc dạng stepper(form wizard) không? 
        // mac dinh id = 0
        id ??= 0;

        ViewData["id"] = id;

        // neu không trả về report code thì chuyển sang link lỗi
        if (FormCode == null)
        {
          return Json(new { success = false, errorMessage = "Không tồn tại mã biểu mẫu" });
        }
        ViewData["FormCode"] = FormCode;

        // Lấy dynamic query parameters
        var parameters = Request.Query;

        // lay thong tin report, va danh sach filter display cua report de xu ly
        var config_form = await _form.NET_Form_Get(FormCode);
        // tra ve page loi neu khong tim thay report
        if (config_form == null)
        {
          return Json(new { success = false, errorMessage = "Không tìm thấy biểu mẫu" });
        }
        // chuyen cau hinh form len giao dien de xu ly
        ViewData["config_form"] = config_form;

        string? connectionString = null;
        //neu datasourceId la null thi lay connectionString mac dinh
        if (config_form.ContainsKey("datasourceid"))
        {
          if (config_form["datasourceid"] != null)
          {
            //lay connectionstring tu cau hinh form de goi store
            connectionString = await _datasrc.GetConnectionString(Convert.ToInt32(config_form["datasourceid"]));
          }
        }

        // khai bao cac du lieu cau hinh form can su dung trong controller
        string? storeDefaultData = config_form.ContainsKey("storedefaultdata") ? Convert.ToString(config_form["storedefaultdata"]) : "";
        string? storeGetData = config_form.ContainsKey("storegetdata") ? Convert.ToString(config_form["storegetdata"]) : "";
        //string? storeSetData = config_form.ContainsKey("StoreSetData") ? Convert.ToString(config_form["StoreSetData"]) : "";

        if (string.IsNullOrWhiteSpace(storeDefaultData) && string.IsNullOrWhiteSpace(storeGetData) == null)
        {
          return Json(new { success = false, errorMessage = "Không tồn tại store lây dữ liệu biểu mẫu" });
        }

        // chuyen parameters cua duong dan thanh Idictionary<string, object>
        Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value.ToString());
        // đưa popup param qua view để chuyển cho các partial nếu có
        ViewData["objParameters"] = objParameters;

        // lay danh sach dynamic field cua form de xu ly
        var stopwatch = Stopwatch.StartNew();
        var config_formfield = await _form.NET_Form_VersionField_WithForm_sel(FormCode, null);
        stopwatch.Stop();
        _logger.LogInformation($"Query config_formfieldTask executed in {stopwatch.ElapsedMilliseconds} ms");

        // chuyển cấu hình form field lên giao diện để xử lý
        ViewData["config_formfield"] = config_formfield;

        // doi voi cac fiekd co kieu select (select box, dropdownbox, tagbox,...), day cac bo select vao SelectListItem va đóng gói trong Dictionary để xử lý trên giao diện
        // Tạo Dictionary chứa SelectList cho từng dropdown (theo DynamicFieldName)
        var config_formfieldService = new Dictionary<string, List<SelectListItem>>();

        //Chuyển kết quả sang Dictionary<string, List<SelectListItem>>
        config_formfieldService = await _netService.NET_Service_GetListSelectListByFormField(config_formfield, objParameters);
        //  Gán danh sach select cho cac filter vào ViewBag
        ViewData["DynamicServiceSelectOptions"] = config_formfieldService;

        // Nhận chuỗi json validate cho validation form
        var validationJson = await _form.NET_Form_GetValidation(FormCode);
        ViewData["fieldvalidation"] = validationJson;

        //khai bao phan tu chua data
        var formData = await _form.Form_sel(objParameters, id, (id == 0 ? storeDefaultData : storeGetData), connectionString);
        ViewData["formData"] = formData;

        // xu ly file
        // Kiểm tra xem form có file nào không
        // lay danh sach object type code tu config form neu co field file uploader
        string attObjectTypeCodes = config_form.ContainsKey("attobjecttypecodes") ? Convert.ToString(config_form["attobjecttypecodes"]) : "";

        if (!string.IsNullOrEmpty(attObjectTypeCodes))
        {
            ViewData["fileUrls"] = await _att.HandleFiles(attObjectTypeCodes, null, id);
        }

        // danh sach service theo booking 
        var reportResultList = await _report.ReportDetail_FromParent("bookingid", (id ?? 0).ToString(), "HS_BookingService_search", null);
        ViewData["reportResultList"] = reportResultList;

        // set readonly form neu co isreadonly = false
        ViewData["IsReadOnly"] = isReadOnly;

        // set containerId
        ViewData["ContainerId"] = containerId;

        // set isStepper kiểm tra có phải dạng form wizard không ở phía view
        ViewData["IsStepper"] = isStepper;

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

        return PartialView("~/Views/Shared/Partial/MainPageLayout/_PopupForm_Partial.cshtml");
      }
      catch (PostgresException ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching form.");
        // Optionally, return an error view
        return View("~/Views/Pages/MiscError.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, exception = ex });
      }
    }

    [HttpPost]
    //[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PopupForm(string? FormCode, int? id, [FromForm] IFormCollection form)
    {
      // reset tempdata error message
      TempData["ErrorMessage"] = null;

      // mac dinh id = 0
      id ??= 0;

      // Nếu bạn cần redirect (ví dụ sau khi lưu), có thể dùng:
      // Tách các form input có tiền tố "q_" vì tiền tố q_ là các query param từ link
      string queryString = ParseDataHelper.GetQueryStringFromForm(form);
      string currentPath = HttpContext.Request.Path;

      // Tách lại query param gốc từ form input "q_" để lọc dữ liệu khi xử lý (vẫn lấy dữ liệu form nhưng không lấy dữ liệu report)
      var queryParamerter = form
          .Where(kv => !kv.Key.Contains("["))
          .ToDictionary(
              kv => kv.Key.Replace("q_",""),
              kv => (object)kv.Value.ToString()
          );

      //var queryParamerter = form
      //    .Where(kv => kv.Key.StartsWith("q_"))
      //    .ToDictionary(
      //        kv => kv.Key.Substring(2),
      //        kv => (object)kv.Value.ToString()
      //    );

      // xử lý form để loại các tiền tố q_ ra khỏi Key
      form = ParseDataHelper.RemovePrefix_FromFormKey(form);

      // lay thong tin report, va danh sach filter display cua report de xu ly
      var config_form = await _form.NET_Form_Get(FormCode);
      // tra ve page loi neu khong tim thay report
      if (config_form == null)
      {
        return Json(new { success = false, errorMessage = "Không tìm thấy biểu mẫu" });
      }

      string? connectionString = null;
      //neu datasourceId la null thi lay connectionString mac dinh
      if (config_form.ContainsKey("datasourceid"))
      {
        if (config_form["datasourceid"] != null)
        {
          //lay connectionstring tu cau hinh form de goi store
          connectionString = await _datasrc.GetConnectionString(Convert.ToInt32(config_form["datasourceid"]));
        }
      }

      // khai bao cac du lieu cau hinh form can su dung trong controller
      string? storeSetData = config_form.ContainsKey("storesetdata") ? Convert.ToString(config_form["storesetdata"]) : "";

      if (string.IsNullOrWhiteSpace(storeSetData))
      {
        return Json(new { success = false, errorMessage = "Không tồn tại store xử lý dữ liệu biểu mẫu" });
      }

      // xu ly file
      // Kiểm tra xem form có file nào không
      // lay danh sach object type code tu config form neu co field file uploader
      string attObjectTypeCodes = config_form.ContainsKey("attobjecttypecodes") ? Convert.ToString(config_form["attobjecttypecodes"]) : "";

      if (!string.IsNullOrEmpty(attObjectTypeCodes))
      {
          await _att.HandleFiles(attObjectTypeCodes, form, id);
      }

      // Convert the IFormCollection to a dictionary of strings
      var formData = form.ToDictionary(
                      pair => pair.Key,
                      pair => (object)pair.Value.ToString()  // Ensure each value is a string (flatten StringValues)
                  );

      //// gui form data len view de hien thi
      //ViewData["formData"] = formData;


      var resultList = await _form.Form_ups(formData, id, storeSetData, connectionString);
      //kiem tra du lieu id tra ve
      var id_return = resultList
      .Where(item => ((IDictionary<string, object>)item).ContainsKey("id"))
      .Select(item => ((IDictionary<string, object>)item)["id"])
      .FirstOrDefault(); // Lọc ra những phần tử có Id

      // neu co gia tri tra ve thi bao thanh cong
      if (id_return != null && int.TryParse(id_return.ToString(), out int num) && num > 0)
      {
        id = Convert.ToInt32(id_return);

        if (!string.IsNullOrEmpty(attObjectTypeCodes))
        {
          // xu ly luu bang attachment
          var saveAttachmentResult = await _att.SaveAttachmentTable(form, id ?? 0);

          // Dùng JsonConvert để chuyển về JObject hoặc dynamic
          var json = JObject.FromObject(saveAttachmentResult); // nếu dùng Newtonsoft.Json
          bool success = json["success"]?.Value<bool>() ?? false;

          if (!success)
          {
            string error = json["errorMessage"]?.ToString();
            return Json(new { success = false, errorMessage = error ?? "Lưu file không thành công" });
          }
        }

        //xu ly report form
        // lấy danh sách report code thuộc form
        string stringaggreportcodes = await _form.NET_Form_GetListReportCode(FormCode);
        // với mỗi report code đang có thì xử lý
        if (!string.IsNullOrWhiteSpace(stringaggreportcodes))
        {
          var reportCodes = stringaggreportcodes.Split(',', StringSplitOptions.RemoveEmptyEntries);

          foreach (var code in reportCodes)
          {
            var reportCode = code.Trim();

            // lay thong tin report de xu ly
            var report = await _report.NET_Report_Get(reportCode);
            // tìm thấy report thì tiếp tục
            if (report != null)
            {
                // khai bao cac du lieu report can su dung trong controller
                string? sqlEditContent = report.ContainsKey("sqleditcontent") ? Convert.ToString(report["sqleditcontent"]) : "";
                // Dictionary để nhóm dữ liệu theo số thứ tự [n]
                // Chuyển đổi dữ liệu sang JSON (loc du lieu form tra ve lay du lieu grid va chuyen thanh json)
                string reportJsonData = await _re.ExtractGridDataToJson(form);
                //end xu ly report form
                var reportResultList = await _re.ReportEditor_Json_Update(queryParamerter, id, reportJsonData, sqlEditContent, null);
                //kiem tra ton tai error message
                // Kiểm tra và nối giá trị của ErrorMessage
                if (_con.CheckForErrors(reportResultList, out string errorMessage))
                {
                  return Json(new { success = false, errorMessage = errorMessage });
                }
            }
          }
        }
        // khong tra ve Id, cung khong tra ve error message thi bao loi chua tra ve id
        return Json(new { success = true, id = id });
      }
      else
      {
        //kiem tra ton tai error message
        // Kiểm tra và nối giá trị của ErrorMessage
        if (_con.CheckForErrors(resultList, out string errorMessage))
        {
          return Json(new { success = false, errorMessage = errorMessage });
        }
        // khong tra ve Id, cung khong tra ve error message thi bao loi chua tra ve id
        else
        {
          return Json(new { success = false, errorMessage = "Chưa trả về Id" });
        }
      }

    }

    [HttpGet]
    public async Task<IActionResult> GetDataFillSelection(string value, string key, string datafillstore)
    {
      // param truyen vao
      var parameters = new Dictionary<string, object>
                {
                    { key, value }
                    // Thêm các tham số khác nếu cần
                };

      //xu ly tra ve data fill tu store
      var data = await _form.Form_GetDataFill_FromSelection(parameters, datafillstore, null);

      Console.WriteLine($"Type of data: {data?.GetType()}"); // Kiểm tra kiểu dữ liệu

      return Ok(data);
    }


    [HttpPost]
    public async Task<IActionResult> ScanCCCD(List<IFormFile> files)
    {
      if (files == null || files.Count == 0)
        return Json(new { success = false, message = "Vui lòng chọn ít nhất 1 file ảnh." });

      try
      {
        string apiKey = _googleCloudVisionApiKey;

        // Danh sách chứa các request con và danh sách lưu tên file tương ứng để map kết quả
        var imageRequests = new List<object>();
        var fileNames = new List<string>();

        // 1. Đóng gói toàn bộ các file ảnh thành các Object trong mảng requests
        foreach (var file in files)
        {
          if (file.Length > 0)
          {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            string base64Image = Convert.ToBase64String(memoryStream.ToArray());

            // Thêm từng ảnh vào danh sách batch theo đúng format Google yêu cầu
            imageRequests.Add(new
            {
              image = new { content = base64Image },
              features = new[] { new { type = "TEXT_DETECTION" } }
            });

            fileNames.Add(file.FileName);
          }
        }

        if (imageRequests.Count == 0)
          return Json(new { success = false, message = "Không có file nào hợp lệ để xử lý." });

        // 2. Gom tất cả vào 1 Payload duy nhất gửi đi theo dạng Batch
        var batchRequestBody = new { requests = imageRequests };
        string jsonPayload = JsonSerializer.Serialize(batchRequestBody);

        // 3. Gửi 1 REQUEST HTTP POST duy nhất chứa toàn bộ các ảnh lên Google
        var client = _httpClientFactory.CreateClient();
        string url = $"https://vision.googleapis.com/v1/images:annotate?key={apiKey}";

        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
          string errorContent = await response.Content.ReadAsStringAsync();
          return Json(new { success = false, message = $"Lỗi từ Google API: {errorContent}" });
        }

        // 4. Đọc dữ liệu mảng kết quả trả về từ Google
        string jsonResponse = await response.Content.ReadAsStringAsync();
        var finalResults = new List<object>();

        using var doc = JsonDocument.Parse(jsonResponse);
        var root = doc.RootElement;

        // Google sẽ trả về mảng "responses" có số lượng và thứ tự khớp 100% với danh sách ảnh gửi lên
        if (root.TryGetProperty("responses", out var responses) && responses.GetArrayLength() > 0)
        {
          for (int i = 0; i < responses.GetArrayLength(); i++)
          {
            string currentFileName = fileNames[i];
            string extractedText = "";
            var singleResponse = responses[i];

            // Kiểm tra xem ảnh này có trích xuất text thành công không
            if (singleResponse.TryGetProperty("textAnnotations", out var textAnnotations) && textAnnotations.GetArrayLength() > 0)
            {
              extractedText = textAnnotations[0].GetProperty("description").GetString();
            }

            // 5. Thừa kế hàm Regex tách chuỗi chuẩn hiện tại của bạn
            dynamic parsedData = ParseCccdData(extractedText);

            finalResults.Add(new
            {
              FileName = currentFileName,
              IdNumber = parsedData.IdNumber,
              FullName = parsedData.FullName,
              Gender = parsedData.Gender,
              BirthDate = parsedData.BirthDate
            });
          }
        }

        // Trả danh sách kết quả về cho Frontend render ra Table
        return Json(new { success = true, results = finalResults });
      }
      catch (Exception ex)
      {
        return Json(new { success = false, message = "Lỗi xử lý Batch OCR: " + ex.Message });
      }
    }

    private object ParseCccdData(string text)
    {
      var result = new { IdNumber = "Không tìm thấy", FullName = "Không tìm thấy", Gender = "Không tìm thấy", BirthDate = "Không tìm thấy" };
      if (string.IsNullOrEmpty(text)) return result;

      // 1. Tìm số CCCD: Quét chuỗi 12 số liên tiếp trên toàn văn bản
      string idNumber = "Không tìm thấy";
      var matchId = Regex.Match(text, @"\b\d{12}\b");
      if (matchId.Success) idNumber = matchId.Value;

      // 2. Tìm Giới tính: Quét từ khóa độc lập bất kể dính dòng
      string gender = "Không tìm thấy";
      if (Regex.IsMatch(text, @"Giới tính\s*/\s*Sex\s*Nữ|Sex\s*Nữ|SexNữ|Nữ", RegexOptions.IgnoreCase)) gender = "Nữ";
      else if (Regex.IsMatch(text, @"Giới tính\s*/\s*Sex\s*Nam|Sex\s*Nam|SexNam|Nam", RegexOptions.IgnoreCase)) gender = "Nam";

      // 3. XỬ LÝ HỌ TÊN THEO KHỐI (BẤT CHẤP XUỐNG DÒNG)
      string fullName = "Không tìm thấy";

      // Khai báo Regex quét KHỐI dựa trên từ khóa rút gọn "name" hoặc "tên"
      // Tận dụng RegexOptions.Singleline để gom toàn bộ các dòng họ tên bị bẻ dòng
      string patternNameBlock = @"(?:name|tên)[:\s/]*(.*?)(?:Ngày|Date|birth|\d{2}/\d{2}/\d{4})";

      var blockMatch = Regex.Match(text, patternNameBlock, RegexOptions.IgnoreCase | RegexOptions.Singleline);

      if (blockMatch.Success)
      {
        string rawNameBlock = blockMatch.Groups[1].Value;

        // Tách khối vừa nhặt được thành các dòng để lọc chữ in hoa
        string[] rawLines = rawNameBlock.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        var validNameParts = new System.Collections.Generic.List<string>();

        foreach (var line in rawLines)
        {
          string cleanLine = line.Trim();

          // BỎ QUA DÒNG RÁC: Nếu từ khóa "name" vô tình bắt trúng đoạn trên, 
          // dòng tiếp theo có thể chứa chữ "CITIZEN" hoặc "CARD", ta sẽ loại ngay.
          if (cleanLine.Contains("CITIZEN") || cleanLine.Contains("IDENTITY") || cleanLine.Contains("CARD"))
            continue;

          if (cleanLine == "/" || cleanLine == ":" || string.IsNullOrEmpty(cleanLine))
            continue;

          // Tiêu chuẩn vàng: Họ tên CCCD bắt buộc phải là chữ IN HOA hoàn toàn
          string noSpace = cleanLine.Replace(" ", "");
          bool isAllUpperCase = noSpace.Length > 0 && noSpace.All(c => !char.IsLetter(c) || char.IsUpper(c));

          if (isAllUpperCase && cleanLine.Length > 1)
          {
            validNameParts.Add(cleanLine);
          }
        }

        if (validNameParts.Count > 0)
        {
          fullName = string.Join(" ", validNameParts).ToUpper().Trim();
        }
      }

      // 4. BỔ SUNG: XỬ LÝ NGÀY SINH (Tìm định dạng ngày tháng ngay sau từ khóa birth/sinh)
      string birthDate = "Không tìm thấy";

      // Quét tìm từ khóa rút gọn "birth" hoặc "sinh", sau đó tìm chuỗi ngày tháng dd/mm/yyyy gần nhất
      var birthMatch = Regex.Match(text, @"(?:birth|sinh)[\s\S]*?(\d{2}/\d{2}/\d{4})", RegexOptions.IgnoreCase);
      if (birthMatch.Success)
      {
        birthDate = birthMatch.Groups[1].Value;
      }
      else
      {
        // Backup case: Nếu OCR tệ đến mức mất luôn chữ "birth" hay "sinh", quét tìm chuỗi ngày tháng đầu tiên xuất hiện trong văn bản
        var backupBirthMatch = Regex.Match(text, @"\b\d{2}/\d{2}/\d{4}\b");
        if (backupBirthMatch.Success) birthDate = backupBirthMatch.Value;
      }

      // Trả về kết quả đã bao gồm Ngày sinh (BirthDate)
      return new { IdNumber = idNumber, FullName = fullName, Gender = gender, BirthDate = birthDate };
    }

    [HttpPost]
    public async Task<IActionResult> AjaxButton([FromForm] IFormCollection form)
    {
      try
      {
        // lay gia tri tu form gui len
        // Convert the IFormCollection to a dictionary of strings
        var formData = form.ToDictionary(
                        pair => pair.Key,
                        pair => (object)pair.Value.ToString()  // Ensure each value is a string (flatten StringValues)
                    );


        string responsefield = formData.ContainsKey("responsefield") ? formData["responsefield"].ToString() : "";

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

        // lay gia tri sql store tu ajax gui len
        int? datasourceid = formData.ContainsKey("datasourceid") ? Convert.ToInt32(formData["datasourceid"]) : null;
        string? connectionString = null;
        //neu datasourceId la null thi lay connectionString mac dinh
        if (datasourceid != null)
        {
          //lay connectionstring tu cau hinh form de goi store
          connectionString = await _datasrc.GetConnectionString((int)datasourceid);
        }


        // xu ly luu form
        var result = await _form.NET_Form_AjaxButtonHandler(formData, sqlstore, connectionString);
        return Json(new { success = true, response = result, responsefield = responsefield });
      }
      catch (PostgresException ex)
      {
        return Json(new { success = false, errorMessage = ex.Message });
      }
    }
    [Authorize]
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult FormBuilder(string FormCode = "HS_Booking_NhapKhachNhanh", bool CreateNew = false)
    {
      if (string.IsNullOrWhiteSpace(FormCode))
      {
        ViewData["ErrorMessage"] = "Không tồn tại mã biểu mẫu";
        return View();
      }

      ViewData["FormCode"] = FormCode.Trim();
      ViewData["CreateNew"] = CreateNew;
      return View();
    }

    [Authorize]
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> FormBuilderData(string FormCode, CancellationToken cancellationToken)
    {
      if (string.IsNullOrWhiteSpace(FormCode))
        return BadRequest(new { success = false, errorMessage = "Mã biểu mẫu không được để trống." });

      var result = await _formBuilder.GetFormBuilderDataAsync(FormCode.Trim(), GetCurrentUserId(), cancellationToken);
      if (!result.Success)
      {
        var missingForm = result.ErrorMessage?.Contains("Không tìm thấy biểu mẫu", StringComparison.OrdinalIgnoreCase) == true;
        return StatusCode(missingForm ? StatusCodes.Status404NotFound : StatusCodes.Status500InternalServerError,
          new { success = false, errorMessage = result.ErrorMessage, formCode = FormCode });
      }

      return Json(new
      {
        success = true,
        errorMessage = result.ErrorMessage,
        formCode = result.FormCode,
        data = result.ConfigJson
      });
    }

    [Authorize]
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> FormBuilderCatalog(CancellationToken cancellationToken)
    {
      var result = await _formBuilder.GetFormBuilderCatalogAsync(GetCurrentSiteId(), GetCurrentUserId(), cancellationToken);
      if (!result.Success)
        return BadRequest(new { success = false, errorMessage = result.ErrorMessage });

      return Json(new { success = true, errorMessage = result.ErrorMessage, data = result.CatalogJson });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(FormBuilderPayloadValidator.MaxPayloadBytes + 65536)]
    public async Task<IActionResult> FormBuilderSave([FromBody] FormBuilderSaveRequest request, CancellationToken cancellationToken)
    {
      var userId = GetCurrentUserId();
      if (!userId.HasValue)
        return Unauthorized(new { success = false, errorMessage = "Không xác định được người dùng đang đăng nhập." });

      if (request == null)
        return BadRequest(new { success = false, errorMessage = "Thiếu payload cấu hình." });

      var validationError = FormBuilderPayloadValidator.Validate(request.Payload);
      if (validationError != null)
        return BadRequest(new { success = false, errorMessage = validationError });

      var result = await _formBuilder.SaveFormBuilderAsync(request.Payload, userId.Value, cancellationToken);
      var response = new
      {
        success = result.Success,
        errorMessage = result.ErrorMessage,
        formId = result.FormId,
        formCode = result.FormCode,
        versionId = result.VersionId,
        version = result.Version,
        status = result.Status,
        savedFieldCount = result.SavedFieldCount,
        savedServiceCount = result.SavedServiceCount,
        warnings = result.Warnings,
        lastModificationTime = result.LastModificationTime
      };

      if (result.Success) return Json(response);
      return StatusCode(result.IsConcurrencyConflict ? StatusCodes.Status409Conflict : StatusCodes.Status400BadRequest, response);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FormBuilderPublish([FromBody] FormBuilderPublishRequest request, CancellationToken cancellationToken)
    {
      var userId = GetCurrentUserId();
      if (!userId.HasValue)
        return Unauthorized(new { success = false, errorMessage = "Không xác định được người dùng đang đăng nhập." });
      if (request.FormId <= 0 || request.VersionId <= 0)
        return BadRequest(new { success = false, errorMessage = "Form ID và Version ID không hợp lệ." });

      var result = await _formBuilder.PublishFormBuilderAsync(request, userId.Value, cancellationToken);
      var response = new
      {
        success = result.Success,
        errorMessage = result.ErrorMessage,
        formId = result.FormId,
        formCode = result.FormCode,
        versionId = result.VersionId,
        version = result.Version,
        status = result.Status,
        warnings = result.Warnings,
        lastModificationTime = result.LastModificationTime
      };

      if (result.Success) return Json(response);
      return StatusCode(result.IsConcurrencyConflict ? StatusCodes.Status409Conflict : StatusCodes.Status400BadRequest, response);
    }

    private int? GetCurrentUserId()
    {
      var raw = User.FindFirst("UserID")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
      return int.TryParse(raw, out var userId) ? userId : null;
    }

    private int? GetCurrentSiteId()
    {
      return int.TryParse(User.FindFirst("SiteId")?.Value, out var siteId) ? siteId : null;
    }
  }
}
