using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcFull.Models;
using KOAHome.EntityFramework;
using KOAHome.Services;
using System.Security.Cryptography;
using Npgsql;
using Microsoft.Data.SqlClient;

namespace AspnetCoreMvcFull.Controllers;

public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;
  private readonly QLKCL_NEWContext _db;
  private readonly IReportEditorService _re;
  private readonly IAttachmentService _att;
  private readonly IReportService _report;
  private readonly IFormService _form;
  private readonly IActionService _action;
  private readonly IWidgetService _widget;
  private readonly IDRDatasourceService _datasrc;
  private readonly IConnectionService _con;

  public HomeController(QLKCL_NEWContext db, ILogger<HomeController> logger, IReportEditorService re, IAttachmentService att, IReportService report, IFormService form, IActionService action, IWidgetService widget, IDRDatasourceService datasrc, IConnectionService con)
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
    _con = con;
  }

  public IActionResult Index()
    {
        return View();
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
      var resultList = await _action.Action_store(formData, sqlstore, connectionString);
      //kiem tra du lieu success tra ve
      var success_return = resultList
      .Where(item => ((IDictionary<string, object>)item).ContainsKey("success"))
      .Select(item => ((IDictionary<string, object>)item)["success"])
      .FirstOrDefault(); // Lọc ra những phần tử có Success

      //kiem tra du lieu error message tra ve
      var errormessage_return = resultList
      .Where(item => ((IDictionary<string, object>)item).ContainsKey("errormessage"))
      .Select(item => ((IDictionary<string, object>)item)["errormessage"])
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
    catch (SqlException ex)
    {
      return Json(new { success = false, errorMessage = ex.Message });
    }
  }

  [HttpGet]
  public async Task<IActionResult> GetNotifications()
  {
    Dictionary<string, object> objParameters = new Dictionary<string, object>();
    //////// Widget List item ty le kin phong trong tuan
    var notifications = await _widget.Widget_GetList(objParameters, "HS_Notify_Search", null);
    return Ok(notifications);
  }



  // xu ly import file excel de cap nhat va them moi du lieu
  [HttpPost]
  public async Task<IActionResult> ImportExcel(IFormCollection form, IFormFile file)
  {
    if (file == null || file.Length == 0)
      return Json(new { success = false, errorMessage = "File is empty" });

    // lay store import tu file
    string? importSqlStore = await _report.ExtractImportDataToStoreName(file);
    // lay store duoc tra ve tu input import
    // lay gia tri sql store tu ajax gui len
    string? sqlstore = form.ContainsKey("sqlstore") ? form["sqlstore"].ToString() : null;
    if (sqlstore == null)
    {
      return Json(new { success = false, errorMessage = "Không tìm thấy store." });
    }
    // compare neu 2 store không khớp thì báo lỗi (tránh import file bậy)
    if (sqlstore != importSqlStore)
    {
      return Json(new { success = false, errorMessage = "File không hợp lệ do không khớp store." });
    }

    //xu ly luu du lieu import
    // Dictionary để nhóm dữ liệu theo số thứ tự [n]
    // Chuyển đổi dữ liệu sang JSON (lay du lieu import va chuyen thanh json)
    string importJsonData = await _report.ExtractImportDataToJson(file);
    //end xu ly luu du lieu import
    // xu ly luu du lieu
    var importResultList = await _report.Import_Json_Update(null, importJsonData, sqlstore, null);
    //kiem tra ton tai error message
    // Kiểm tra và nối giá trị của ErrorMessage
    if (_con.CheckForErrors(importResultList, out string errorMessage))
    {
      return Json(new { success = false, errorMessage = errorMessage });
    }
    // khong tra ve Id, cung khong tra ve error message thi bao loi chua tra ve id
    else
    {
      return Json(new { success = true });
    }
  }

  [HttpPost("upload")]
  public async Task<IActionResult> Upload(IFormFile file)
  {
    if (file != null && file.Length > 0)
    {
      var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
      if (!Directory.Exists(uploads))
        Directory.CreateDirectory(uploads);

      var filePath = Path.Combine(uploads, file.FileName);
      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await file.CopyToAsync(stream);
      }

      return Ok(new { success = true, fileName = file.FileName });
    }

    return BadRequest(new { success = false });
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
