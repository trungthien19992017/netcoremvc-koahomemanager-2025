using KOAHome.Controllers;
using KOAHome.EntityFramework;
using KOAHome.Helpers;
using KOAHome.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace KOAHome.ViewComponents
{
  public class NETFormViewComponent : ViewComponent
  {
    private readonly ILogger<NETFormViewComponent> _logger;
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


    public NETFormViewComponent(QLKCL_NEWContext db, ILogger<NETFormViewComponent> logger, IHsBookingTableService book, IHsBookingServiceService bookser, IReportEditorService re, IAttachmentService att, IHsCustomerService cus, IReportService report, IFormService form, IActionService action, IWidgetService widget, IDRDatasourceService datasrc, INetServiceService netService, IConnectionService con)
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

    public async Task<IViewComponentResult> InvokeAsync(int? id, string FormCode, bool? isReadOnly = false)
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
          return View("PopupForm");
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
          return View("PopupForm");
        }
        // chuyen cau hinh form len giao dien de xu ly
        ViewData["config_form"] = config_form;

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

        if (string.IsNullOrWhiteSpace(storeDefaultData) && string.IsNullOrWhiteSpace(storeGetData) == null)
        {
          ViewData["ErrorMessage"] = "Không tồn tại store lây dữ liệu biểu mẫu";
          return View("PopupForm");
        }

        // chuyen parameters cua bo loc thanh Idictionary<string, object>
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
        string attObjectTypeCodes = config_form.ContainsKey("AttObjectTypeCodes") ? Convert.ToString(config_form["AttObjectTypeCodes"]) : "";
        ViewData["fileUrls"] = await _att.HandleFiles(attObjectTypeCodes, null, id);

        // danh sach service theo booking 
        var reportResultList = await _report.ReportDetail_FromParent("BookingID", (id ?? 0).ToString(), "HS_BookingService_search", null);
        ViewData["reportResultList"] = reportResultList;

        // set readonly form neu co isreadonly = false
        ViewData["IsReadOnly"] = isReadOnly;

        // dùng tạm để test dynamic report
        ViewData["ServiceId"] = new SelectList(_db.HsServices.Where(p => p.IsActive == true).OrderBy(p => p.OrderId), "ServiceId", "Name");

        // neu co loi tu action POST tra ve thi bao loi
        if (TempData["ErrorMessage"] != null)
        {
          ViewData["ErrorMessage"] = TempData["ErrorMessage"];
          return View("PopupForm");
        }
        else
        {
          //khai bao success
          ViewData["success"] = "Thành công";
        }

        return View("PopupForm");
      }
      catch (Exception ex)
      {
        // Log the exception
        _logger.LogError(ex, "Lỗi khi render ViewComponent DynamicForm với formCode: {FormCode}, id: {Id}", FormCode, id);

        // Có thể trả về View lỗi riêng hoặc HTML báo lỗi
        return View("Error", $"Không thể tải form '{FormCode}'. Vui lòng thử lại sau.");
      }

      //var data = await LoadDataAsync(id, formCode, queryParams);
      //return View("PopupForm", data);
    }

    private Task<string> GetFormAsync(int? id, string FormCode, string queryParams, bool? isReadOnly)
    {
      // Lỗi thử nghiệm
      if (id == null || id == 0)
        throw new ArgumentNullException(nameof(id));

      return Task.FromResult($"Form động: {FormCode} - ID: {id} - Params: {queryParams} - isReadOnly: {isReadOnly}" );
    }
  }
}
