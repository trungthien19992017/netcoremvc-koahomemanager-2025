using AspnetCoreMvcFull.Models;
using Google.Apis.Sheets.v4.Data;
using KOAHome.EntityFramework;
using KOAHome.Models;
using KOAHome.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace AspnetCoreMvcFull.Controllers;

public class DashboardsController : Controller
{
  private readonly ILogger<DashboardsController> _logger;
  private readonly QLKCL_NEWContext _db;
  private readonly IWidgetService _widget;
  private readonly IReportService _reportService;
  private readonly IGoogleSheetService _googleSheetService;


  public DashboardsController(ILogger<DashboardsController> logger, IWidgetService widget, IReportService reportService, IGoogleSheetService googleSheetService)
  {
    _logger = logger;
    _widget = widget;
    _reportService = reportService;
    _googleSheetService = googleSheetService;
  }

  public async Task<IActionResult> Index()
  {
    return View();
  }

  [HttpGet]
  public async Task<IActionResult> KoaDashboard([FromQuery] Dictionary<string, string> parameters)
  {
    // xu ly bo loc
    // chuyen parameters thanh Idictionary<string, object>
    Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);

    // xu ly lay du lieu cho tung widget
    //khai bao phan tu chua data

    ////////widget simple card Chuc mung
    ////Tổng doanh thu tháng
    var SimpleCard_ChucMung = await _widget.Widget_GetObject(objParameters, "HS_Widget_SimpleCard_ChucMung", null);
    ViewBag.SimpleCard_ChucMung = SimpleCard_ChucMung;

    ////////widget simple cart So lieu trong thang
    //Doanh thu, luot book, so gio, chi
    var SimpleCard_SoLieuTrongThang = await _widget.Widget_GetObject(objParameters, "HS_Widget_SimpleCard_SoLieuTrongThang", null);
    ViewBag.SimpleCard_SoLieuTrongThang = SimpleCard_SoLieuTrongThang;

    ////////line chart doanh thu 6 thang gan day
    var LineChart_DoanhThuCacThang = await _widget.Widget_GetObject(objParameters, "HS_Widget_LineChart_DoanhThuCacThang", null);
    ViewBag.LineChart_DoanhThuCacThang = LineChart_DoanhThuCacThang;

    ////////Column chart chi phi 6 thang gan day
    var ColumnChart_ChiPhiCacThang = await _widget.Widget_GetObject(objParameters, "HS_Widget_ColumnChart_ChiPhiCacThang", null);
    ViewBag.ColumnChart_ChiPhiCacThang = ColumnChart_ChiPhiCacThang;

    //////// List item Top 5 dịch vụ tháng
    var ListItem_TopDichVuThang = await _widget.Widget_GetList(objParameters, "HS_Widget_ListItem_TopDichVuThang", null);
    ViewBag.ListItem_TopDichVuThang = ListItem_TopDichVuThang;

    //////// Pie Chart tỷ lệ các phòng trong tháng
    var PieChart_TyLeCacPhongTrongThang = await _widget.Widget_GetObject(objParameters, "HS_Widget_PieChart_TyLeCacPhongTrongThang", null);
    ViewBag.PieChart_TyLeCacPhongTrongThang = PieChart_TyLeCacPhongTrongThang;

    //////// Widget List item ty le kin phong trong tuan
    var ListItem_TyLeKinPhongTuan = await _widget.Widget_GetList(objParameters, "HS_Widget_ListItem_TyLeKinPhongTuan", null);
    ViewBag.ListItem_TyLeKinPhongTuan = ListItem_TyLeKinPhongTuan;

    //////// Widget list item Top 5 khách hàng gần đây nhất
    var ListItem_TopKhachHangGanDay = await _widget.Widget_GetList(objParameters, "HS_Widget_ListItem_TopKhachHangGanDay", null);
    ViewBag.ListItem_TopKhachHangGanDay = ListItem_TopKhachHangGanDay;

    //////// Column chart Doanh thu tuần
    var ColumnChart_DoanhThuTuan = await _widget.Widget_GetObject(objParameters, "HS_Widget_ColumnChart_DoanhThuTuan", null);
    ViewBag.ColumnChart_DoanhThuTuan = ColumnChart_DoanhThuTuan;

    //////// Heat map Trang Thai dat phong trong thang
    var HeatMap_TrangThaiDatPhongThang = await _widget.Widget_GetList(objParameters, "HS_Widget_HeatMap_trangThaiDatPhongThang", null);

    // B2: nhóm theo DayOfWeekName để tạo từng dòng (series)
    var grouped = HeatMap_TrangThaiDatPhongThang
        .GroupBy(d => (string)d.dayofweekname)
        .Select(g => new
        {
          name = g.Key, // SUN, MON,...
          data = g.Select(item => new {
            x = (string)item.weekname,
            y = (decimal)item.revenue
          }).ToList()
        }).ToList();

    // B3: truyền ra View qua ViewBag hoặc ViewData
    ViewBag.HeatMap_TrangThaiDatPhongThang = JsonSerializer.Serialize(grouped);

    return View();
  }

  [HttpGet]
  public async Task<IActionResult> DragdropComponentReview()
  {
    return View();
  }

  [HttpGet]
  public async Task<IActionResult> DragdropComponentReview1([FromQuery] Dictionary<string, string> parameters)
  {
    // xu ly bo loc
    // chuyen parameters thanh Idictionary<string, object>
    Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);

    // xu ly lay du lieu cho tung widget
    //khai bao phan tu chua data

    ////////widget simple card Chuc mung
    ////Tổng doanh thu tháng
    var SimpleCard_ChucMung = await _widget.Widget_GetObject(objParameters, "HS_Widget_SimpleCard_ChucMung", null);
    ViewBag.SimpleCard_ChucMung = SimpleCard_ChucMung;

    ////////widget simple cart So lieu trong thang
    //Doanh thu, luot book, so gio, chi
    var SimpleCard_SoLieuTrongThang = await _widget.Widget_GetObject(objParameters, "HS_Widget_SimpleCard_SoLieuTrongThang", null);
    ViewBag.SimpleCard_SoLieuTrongThang = SimpleCard_SoLieuTrongThang;

    ////////line chart doanh thu 6 thang gan day
    var LineChart_DoanhThuCacThang = await _widget.Widget_GetObject(objParameters, "HS_Widget_LineChart_DoanhThuCacThang", null);
    ViewBag.LineChart_DoanhThuCacThang = LineChart_DoanhThuCacThang;

    ////////Column chart chi phi 6 thang gan day
    var ColumnChart_ChiPhiCacThang = await _widget.Widget_GetObject(objParameters, "HS_Widget_ColumnChart_ChiPhiCacThang", null);
    ViewBag.ColumnChart_ChiPhiCacThang = ColumnChart_ChiPhiCacThang;

    //////// List item Top 5 dịch vụ tháng
    var ListItem_TopDichVuThang = await _widget.Widget_GetList(objParameters, "HS_Widget_ListItem_TopDichVuThang", null);
    ViewBag.ListItem_TopDichVuThang = ListItem_TopDichVuThang;

    //////// Pie Chart tỷ lệ các phòng trong tháng
    var PieChart_TyLeCacPhongTrongThang = await _widget.Widget_GetObject(objParameters, "HS_Widget_PieChart_TyLeCacPhongTrongThang", null);
    ViewBag.PieChart_TyLeCacPhongTrongThang = PieChart_TyLeCacPhongTrongThang;

    //////// Widget List item ty le kin phong trong tuan
    var ListItem_TyLeKinPhongTuan = await _widget.Widget_GetList(objParameters, "HS_Widget_ListItem_TyLeKinPhongTuan", null);
    ViewBag.ListItem_TyLeKinPhongTuan = ListItem_TyLeKinPhongTuan;

    //////// Widget list item Top 5 khách hàng gần đây nhất
    var ListItem_TopKhachHangGanDay = await _widget.Widget_GetList(objParameters, "HS_Widget_ListItem_TopKhachHangGanDay", null);
    ViewBag.ListItem_TopKhachHangGanDay = ListItem_TopKhachHangGanDay;

    //////// Column chart Doanh thu tuần
    var ColumnChart_DoanhThuTuan = await _widget.Widget_GetObject(objParameters, "HS_Widget_ColumnChart_DoanhThuTuan", null);
    ViewBag.ColumnChart_DoanhThuTuan = ColumnChart_DoanhThuTuan;

    return View();
  }

  [HttpPost]
  public async Task<IActionResult> DragdropComponentReview1([FromBody] List<WidgetLayout> layouts)
  {
    // layouts là danh sách các widget gồm id, order, width, height

    // TODO: lưu layouts vào database, session hoặc file

    // Ví dụ: log thử xem server nhận được gì
    foreach (var item in layouts)
    {
      Console.WriteLine($"Widget ID: {item.Id}, Order: {item.Order}, Width: {item.Width}, Height: {item.Height}");
    }

    return Ok(new { message = "Layout saved successfully" });
  }


  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
  [HttpPost]
  public async Task<IActionResult> SyncGoogleSheet()
  {
    try
    {
      // 1. Lấy dữ liệu từ DB
      var today = DateTime.Today;

      // ISO week: Monday = 1, Sunday = 7
      int diffToMonday = today.DayOfWeek == DayOfWeek.Sunday
          ? -6
          : DayOfWeek.Monday - today.DayOfWeek;

      DateTime startOfWeek = today.AddDays(diffToMonday);
      DateTime endOfWeek = startOfWeek.AddDays(6);

      string tuNgay = startOfWeek.ToString("yyyy-MM-dd");
      string denNgay = endOfWeek.ToString("yyyy-MM-dd");

      var parameters = new Dictionary<string, object>
      {
        ["tungay"] = tuNgay,
        ["denngay"] = denNgay,
        ["param"] = $"tungay={tuNgay};denngay={denNgay}"
      };

      // 2️⃣ GỌI STORE / SQL
      var result = await _reportService.Report_search(
          parameters,
          sqlStore: "HS_LichBookingThang_GoogleSheet",
          connectionString: "Server=aws-0-ap-southeast-1.pooler.supabase.com;Port=5432;Database=TTT_Bus_KOAHome;User Id=postgres.degfqiahqjcmvgdkcjmm;Password=15976325Vn.;SSL Mode=Require;Trust Server Certificate=true;Pooling=true;Minimum Pool Size=1;Maximum Pool Size=20;"
      );

      // 3️⃣ CHUYỂN dynamic → Dictionary<string, object>
      var data = ConvertDynamicToDictionary(result);
      var headers = data.First().Keys.ToList();
      int totalColumns = headers.Count;

      string spreadsheetId = "1VgUes_otZNcWHm_UOErhDjPkYs2sWWhz94lvIKDIx9Q";
      var sheetId = await _googleSheetService.GetSheetIdByName(
          spreadsheetId,
          "Sheet1"
      );

      // 2. Ghi lên Google Sheet
      _googleSheetService.WriteDictionaryToSheet(
          spreadsheetId: spreadsheetId,
          sheetName: "Sheet1",
          data: data
      );
      await _googleSheetService.FormatSheet(
          spreadsheetId,
          sheetId,
          totalColumns: totalColumns,
          totalRows: data.Count + 1
      );

      return Json(new
      {
        success = true,
        message = "Đồng bộ Google Sheet thành công"
      });
    }
    catch (Exception ex)
    {
      return Json(new
      {
        success = false,
        message = ex.Message
      });
    }
  }
  private List<Dictionary<string, object>> ConvertDynamicToDictionary(
      List<dynamic> rows)
  {
    return rows.Select(r =>
        ((IDictionary<string, object>)r)
        .ToDictionary(x => x.Key, x => x.Value)
    ).ToList();
  }
}
