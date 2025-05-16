using AspnetCoreMvcFull.Models;
using KOAHome.EntityFramework;
using KOAHome.Helpers;
using KOAHome.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

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
    private readonly IActionService _action;
    private readonly IWidgetService _widget;
    private readonly IDRDatasourceService _datasrc;
    private readonly INetServiceService _netService;
    private readonly IConnectionService _con;

    public NETFormController(QLKCL_NEWContext db, ILogger<NETFormController> logger, IReportEditorService re, IAttachmentService att, IReportService report, IFormService form, IActionService action, IWidgetService widget, IDRDatasourceService datasrc, INetServiceService netService, IConnectionService con)
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

    // GET: NETFormController
    public ActionResult Index()
    {
      return View();
    }

    // GET: HsBookings/Edit/5
    public async Task<IActionResult> Viewer(string? FormCode, int? id, bool isReadOnly = false)
    {
      try
      {
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

        //khai bao phan tu chua data
        var formData = await _form.Form_sel(objParameters, id, (id == 0 ? storeDefaultData : storeGetData), connectionString);
        ViewData["formData"] = formData;

        // xu ly file
        // Kiểm tra xem form có file nào không
        // lay danh sach object type code tu config form neu co field file uploader
        string attObjectTypeCodes = config_form.ContainsKey("attobjecttypecodes") ? Convert.ToString(config_form["attobjecttypecodes"]) : "";
        ViewData["fileUrls"] = await _att.HandleFiles(attObjectTypeCodes, null, id);

        // danh sach service theo booking 
        var reportResultList = await _report.ReportDetail_FromParent("bookingid", (id ?? 0).ToString(), "HS_BookingService_search", null);
        ViewData["reportResultList"] = reportResultList;

        // set readonly form neu co isreadonly = false
        ViewData["IsReadOnly"] = isReadOnly;

        // dùng tạm để test dynamic report
        ViewData["ServiceId"] = new SelectList(_db.HsServices.Where(p => p.Isactive == true).OrderBy(p => p.Orderid), "serviceid", "name");

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
      catch (Exception ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching form.");
        // Optionally, return an error view
        return View("~/Views/Pages/MiscError.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Exception = ex });
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

      // dùng tạm để test dynamic report
      ViewData["ServiceId"] = new SelectList(_db.HsServices.Where(p => p.Isactive == true).OrderBy(p => p.Orderid), "serviceid", "name");

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

      if (string.IsNullOrWhiteSpace(storeSetData))
      {
        TempData["ErrorMessage"] = "Không tồn tại store xử lý dữ liệu biểu mẫu";
        return Redirect($"{currentPath}?{queryString}");
      }

      // xu ly file
      // Kiểm tra xem form có file nào không
      // lay danh sach object type code tu config form neu co field file uploader
      string attObjectTypeCodes = config_form.ContainsKey("attobjecttypecodes") ? Convert.ToString(config_form["attobjecttypecodes"]) : "";

      await _att.HandleFiles(attObjectTypeCodes, form, id);

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
        id = (int)id_return;

        // xu ly luu bang attachment
        bool isSaveAttachment = await _att.SaveAttachmentTable(form, id ?? 0);

        if (!isSaveAttachment)
        {
          TempData["ErrorMessage"] = "Lưu file không thành công";
          return Redirect($"{currentPath}?{queryString}");
        }

        //xu ly report form
        // Dictionary để nhóm dữ liệu theo số thứ tự [n]
        // Chuyển đổi dữ liệu sang JSON (loc du lieu form tra ve lay du lieu grid va chuyen thanh json)
        string reportJsonData = await _re.ExtractGridDataToJson(form);
        //end xu ly report form
        var reportResultList = await _re.ReportEditor_Json_Update(queryParamerter, id, reportJsonData, "HS_BookingService_Json_ups", null);
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

    // popup form view component
    [HttpGet]
    public async Task<IActionResult> PopupForm(string FormCode, int? id, bool? isReadOnly = false)
    {
      try
      { 
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

        //khai bao phan tu chua data
        var formData = await _form.Form_sel(objParameters, id, (id == 0 ? storeDefaultData : storeGetData), connectionString);
        ViewData["formData"] = formData;

        // xu ly file
        // Kiểm tra xem form có file nào không
        // lay danh sach object type code tu config form neu co field file uploader
        string attObjectTypeCodes = config_form.ContainsKey("attobjecttypecodes") ? Convert.ToString(config_form["attobjecttypecodes"]) : "";
        ViewData["fileUrls"] = await _att.HandleFiles(attObjectTypeCodes, null, id);

        // danh sach service theo booking 
        var reportResultList = await _report.ReportDetail_FromParent("bookingid", (id ?? 0).ToString(), "HS_BookingService_search", null);
        ViewData["reportResultList"] = reportResultList;

        // set readonly form neu co isreadonly = false
        ViewData["IsReadOnly"] = isReadOnly;

        // dùng tạm để test dynamic report
        ViewData["ServiceId"] = new SelectList(_db.HsServices.Where(p => p.Isactive == true).OrderBy(p => p.Orderid), "serviceid", "name");

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
      catch (Exception ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching form.");
        // Optionally, return an error view
        return View("~/Views/Pages/MiscError.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Exception = ex });
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

      // Tách lại query param gốc từ form input "q_" để lọc dữ liệu khi xử lý
      var queryParamerter = form
          .Where(kv => kv.Key.StartsWith("q_"))
          .ToDictionary(
              kv => kv.Key.Substring(2),
              kv => (object)kv.Value.ToString()
          );

      // xử lý form để loại các tiền tố q_ ra khỏi Key
      form = ParseDataHelper.RemovePrefix_FromFormKey(form);

      // dùng tạm để test dynamic report
      ViewData["ServiceId"] = new SelectList(_db.HsServices.Where(p => p.Isactive == true).OrderBy(p => p.Orderid), "serviceid", "name");

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
        id = (int)id_return;

        // xu ly luu bang attachment
        bool isSaveAttachment = await _att.SaveAttachmentTable(form, id ?? 0);

        // xu ly file
        // Kiểm tra xem form có file nào không
        // lay danh sach object type code tu config form neu co field file uploader
        attObjectTypeCodes = config_form.ContainsKey("attobjecttypecodes") ? Convert.ToString(config_form["attobjecttypecodes"]) : "";

        if (!isSaveAttachment)
        {
          return Json(new { success = false, errorMessage = "Lưu file không thành công" });
        }

        //xu ly report form
        // Dictionary để nhóm dữ liệu theo số thứ tự [n]
        // Chuyển đổi dữ liệu sang JSON (loc du lieu form tra ve lay du lieu grid va chuyen thanh json)
        string reportJsonData = await _re.ExtractGridDataToJson(form);
        //end xu ly report form
        var reportResultList = await _re.ReportEditor_Json_Update(queryParamerter, id, reportJsonData, "HS_BookingService_Json_ups", null);
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
  }
}
