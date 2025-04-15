using KOAHome.EntityFramework;
using KOAHome.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace KOAHome.Controllers
{
  public class NETFormController : Controller
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

    public NETFormController(QLKCL_NEWContext db, ILogger<HsBookingsController> logger, IHsBookingTableService book, IHsBookingServiceService bookser, IReportEditorService re, IAttachmentService att, IHsCustomerService cus, IReportService report, IFormService form, IActionService action, IWidgetService widget, IDRDatasourceService datasrc, INetServiceService netService)
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
    // GET: NETFormController
    public ActionResult Index()
    {
      return View();
    }

    // GET: HsBookings/Edit/5
    public async Task<IActionResult> Viewer(string? FormCode, int? id, [FromQuery] Dictionary<string, string> parameters, bool isReadOnly = false)
    {
      try
      {
        // mac dinh id = 0
        id ??= 0;

        ViewBag.id = id;

        // neu không trả về report code thì chuyển sang link lỗi
        if (FormCode == null)
        {
          return RedirectToAction("MiscError", "Pages");
        }
        ViewBag.FormCode = FormCode;

        // lay thong tin report, va danh sach filter display cua report de xu ly
        var config_form = await _form.NET_Form_Get(FormCode);
        // tra ve page loi neu khong tim thay report
        if (config_form == null)
        {
          return RedirectToAction("MiscError", "Pages", new { errorMessage = "Không tìm thấy biểu mẫu" });
        }
        // chuyen cau hinh form len giao dien de xu ly
        ViewBag.config_form = config_form;

        string? connectionString = null;
        //neu datasourceId la null thi lay connectionString mac dinh
        if (config_form.ContainsKey("DatasourceId"))
        {
          if (config_form["DatasourceId"] != null)
          {
            //lay connectionstring tu cau hinh form de goi store
            connectionString = await _datasrc.GetConnectionString(Convert.ToInt32(config_form["DatasourceId"]));
          }
        }

        // khai bao cac du lieu cau hinh form can su dung trong controller
        string? storeDefaultData = config_form.ContainsKey("StoreDefaultData") ? Convert.ToString(config_form["StoreDefaultData"]) : "";
        string? storeGetData = config_form.ContainsKey("StoreGetData") ? Convert.ToString(config_form["StoreGetData"]) : "";
        //string? storeSetData = config_form.ContainsKey("StoreSetData") ? Convert.ToString(config_form["StoreSetData"]) : "";

        if (storeDefaultData == null && storeGetData == null)
        {
          return RedirectToAction("MiscError", "Pages", new { errorMessage = "Không tồn tại store lây dữ liệu biểu mẫu" });
        }

        // chuyen parameters cua duong dan thanh Idictionary<string, object>
        Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);

        // lay danh sach dynamic field cua form de xu ly
        var stopwatch = Stopwatch.StartNew();
        var config_formfield = await _form.NET_Form_VersionField_WithForm_sel(FormCode, null);
        stopwatch.Stop();
        _logger.LogInformation($"Query config_formfieldTask executed in {stopwatch.ElapsedMilliseconds} ms");

        // chuyển cấu hình form field lên giao diện để xử lý
        ViewBag.config_formfield = config_formfield;

        // doi voi cac fiekd co kieu select (select box, dropdownbox, tagbox,...), day cac bo select vao SelectListItem va đóng gói trong Dictionary để xử lý trên giao diện
        // Tạo Dictionary chứa SelectList cho từng dropdown (theo DynamicFieldName)
        var config_formfieldService = new Dictionary<string, List<SelectListItem>>();

        //Chuyển kết quả sang Dictionary<string, List<SelectListItem>>
        config_formfieldService = await _netService.NET_Service_GetListSelectListByFormField(config_formfield, objParameters);
        //  Gán danh sach select cho cac filter vào ViewBag
        ViewBag.DynamicServiceSelectOptions = config_formfieldService;

        //khai bao phan tu chua data
        var formData = await _form.Form_sel(id, (id == 0 ? storeDefaultData : storeGetData), connectionString);
        ViewBag.formData = formData;


        // xu ly cac du lieu select box va radio tren giao dien
        ViewData["CustomerId"] = new SelectList(
                                      _db.HsCustomers.Select(c => new
                                      {
                                        CustomerId = c.CustomerId,
                                        DisplayText = c.LastName + " - " + c.PhoneNumber
                                      }),
                                      "CustomerId",
                                      "DisplayText",
                                      (formData.ContainsKey("CustomerID") ? formData["CustomerID"] : "")
                                  );
        ViewData["RoomId"] = new SelectList(_db.HsRooms, "RoomId", "Name", (formData.ContainsKey("RoomID") ? formData["RoomID"] : ""));
        ViewData["ServiceId"] = new SelectList(_db.HsServices.Where(p => p.IsActive == true).OrderBy(p => p.OrderId), "ServiceId", "Name");
        ViewData["Gender"] = new SelectList(_db.CategoryDetails.Where(p => p.CategoryCode == "Gender"), "Id", "Name");

        // xu ly file
        // Kiểm tra xem form có file nào không
        await HandleFiles("KOAAttachment", null, id);

        // danh sach service theo booking 
        var bookSerResultList = await _report.ReportDetail_FromParent("BookingID", (id ?? 0).ToString(), "HS_BookingService_search", null);
        ViewBag.listbookserbybookingid_store = bookSerResultList;

        // set readonly form neu co isreadonly = false
        ViewBag.IsReadOnly = isReadOnly;

        return View();
      }
      catch (Exception ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching form.");
        // Optionally, return an error view
        return View("Error");
      }

    }

    // POST: HsBookings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost("/form/viewer/{FormCode}/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Viewer(string? FormCode, int? id, [FromForm] IFormCollection form)
    {
      // mac dinh id = 0
      id ??= 0;

      // du lieu show tren giao dien
      ViewData["CustomerId"] = new SelectList(
                                    _db.HsCustomers.Select(c => new
                                    {
                                      CustomerId = c.CustomerId,
                                      DisplayText = c.LastName + " - " + c.PhoneNumber
                                    }),
                                    "CustomerId",
                                    "DisplayText",
                                    form["CustomerID"]
                                );
      ViewData["RoomId"] = new SelectList(_db.HsRooms, "RoomId", "Name", form["RoomID"]);
      ViewData["ServiceId"] = new SelectList(_db.HsServices.Where(p => p.IsActive == true).OrderBy(p => p.OrderId), "ServiceId", "Name");
      ViewData["Gender"] = new SelectList(_db.CategoryDetails.Where(p => p.CategoryCode == "Gender"), "Id", "Name");
      // danh sach service theo booking (khong co du lieu)
      var bookSerResultList = await _report.ReportDetail_FromParent("BookingID", id.ToString(), "HS_BookingService_search", null);
      ViewBag.listbookserbybookingid_store = bookSerResultList;

      // xu ly file
      // Kiểm tra xem form có file nào không
      await HandleFiles("KOAAttachment", form, id);

      // Convert the IFormCollection to a dictionary of strings
      var formData = form.ToDictionary(
                      pair => pair.Key,
                      pair => (object)pair.Value.ToString()  // Ensure each value is a string (flatten StringValues)
                  );

      // gui form data len view de hien thi
      ViewBag.formData = formData;

      var resultList = await _form.Form_ups(formData, id, "HS_Booking1_ups", null);
      //kiem tra du lieu id tra ve
      var id_return = resultList
      .Where(item => ((IDictionary<string, object>)item).ContainsKey("Id"))
      .Select(item => ((IDictionary<string, object>)item)["Id"])
      .FirstOrDefault(); // Lọc ra những phần tử có Id

      // neu co gia tri tra ve thi bao thanh cong
      if (id_return != null && int.TryParse(id_return.ToString(), out int num) && num > 0)
      {
        id = (int)id_return;

        // xu ly luu bang attachment
        bool isSaveAttachment = await _att.SaveAttachmentTable(form, id ?? 0);

        // xu ly file
        // Kiểm tra xem form có file nào không
        await HandleFiles("KOAAttachment", null, id);

        if (!isSaveAttachment)
        {
          ViewBag.ErrorMessage = "Lưu file không thành công";
        }

        //xu ly report form
        // Dictionary để nhóm dữ liệu theo số thứ tự [n]
        // Chuyển đổi dữ liệu sang JSON (loc du lieu form tra ve lay du lieu grid va chuyen thanh json)
        string reportJsonData = await _re.ExtractGridDataToJson(form);
        //end xu ly report form
        var reportResultList = await _re.ReportEditor_Json_Update(id, reportJsonData, "HS_BookingService_Json_ups", null);
        //kiem tra ton tai error message
        // Kiểm tra và nối giá trị của ErrorMessage
        if (CheckForErrors(reportResultList, out string errorMessage))
        {
          ViewBag.ErrorMessage = errorMessage;
          return View();
        }
        // khong tra ve Id, cung khong tra ve error message thi bao loi chua tra ve id
        else
        {

          bookSerResultList = await _report.ReportDetail_FromParent("BookingID", id.ToString(), "HS_BookingService_search", null);
          ViewBag.listbookserbybookingid_store = bookSerResultList;
          //khai bao phan tu chua data
          formData = (Dictionary<string, object>)await _form.Form_sel(id, "HS_Booking1_sel", null);
          ViewBag.formData = formData;

          ViewBag.success = "Xử lý thành công"; // Gán vào ViewBag
          return View();
        }
      }
      else
      {
        //kiem tra ton tai error message
        // Kiểm tra và nối giá trị của ErrorMessage
        if (CheckForErrors(resultList, out string errorMessage))
        {
          ViewBag.ErrorMessage = errorMessage;
          return View();
        }
        // khong tra ve Id, cung khong tra ve error message thi bao loi chua tra ve id
        else
        {
          ViewBag.ErrorMessage = "Chưa trả về Id";
          return View();
        }
      }
    }
  }
}
