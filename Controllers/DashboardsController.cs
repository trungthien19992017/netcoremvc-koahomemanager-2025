using AspnetCoreMvcFull.Models;
using Google.Apis.Sheets.v4.Data;
using Google.Cloud.Vision.V1;
using KOAHome.EntityFramework;
using KOAHome.Models;
using KOAHome.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Tesseract;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AspnetCoreMvcFull.Controllers;

public class DashboardsController : Controller
{
  private readonly ILogger<DashboardsController> _logger;
  private readonly QLKCL_NEWContext _db;
  private readonly IWidgetService _widget;
  private readonly IReportService _reportService;
  private readonly IGoogleSheetService _googleSheetService;
  private readonly string _tessdataPath;
  private readonly IHttpClientFactory _httpClientFactory;
  private readonly string _googleCloudVisionApiKey;


  public DashboardsController(ILogger<DashboardsController> logger, IWidgetService widget, IReportService reportService, IGoogleSheetService googleSheetService, IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory, IConfiguration configuration)
  {
    _logger = logger;
    _widget = widget;
    _reportService = reportService;
    _googleSheetService = googleSheetService;
    _tessdataPath = Path.Combine(webHostEnvironment.WebRootPath, "tessdata");
    _httpClientFactory = httpClientFactory;
    _googleCloudVisionApiKey = configuration["Google:CloudVisionApiKey"];
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
      var hcmZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
      var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, hcmZone).Date;

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
      await _googleSheetService.WriteDictionaryToSheet(
          spreadsheetId: spreadsheetId,
          sheetName: "Sheet1",
          data: data
      );
      await _googleSheetService.ApplyRichTextFromHtml(
          spreadsheetId,
          sheetId,
          startRowIndex: 1,
          totalRows: data.Count + 1,
          totalColumns: data.First().Count
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

  [HttpGet]
  public IActionResult CCCDScanner()
  {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> ScanCard(IFormFile file)
  {
    if (file == null || file.Length == 0)
      return Json(new { success = false, message = "File không hợp lệ." });

    try
    {
      string apiKey = _googleCloudVisionApiKey;

      // 1. Đọc file ảnh chuyển thành chuỗi Base64
      using var memoryStream = new MemoryStream();
      await file.CopyToAsync(memoryStream);
      string base64Image = Convert.ToBase64String(memoryStream.ToArray());

      // 2. Tạo Body Request theo đúng chuẩn JSON của Google Vision API
      var requestBody = new
      {
        requests = new[]
          {
                        new
                        {
                            image = new { content = base64Image },
                            features = new[] { new { type = "TEXT_DETECTION" } }
                        }
                    }
      };

      string jsonPayload = JsonSerializer.Serialize(requestBody);

      // 3. Gọi HTTP POST lên Google API bằng API Key đính kèm trên URL
      var client = _httpClientFactory.CreateClient();
      string url = $"https://vision.googleapis.com/v1/images:annotate?key={apiKey}";

      var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
      var response = await client.PostAsync(url, content);

      if (!response.IsSuccessStatusCode)
      {
        string errorContent = await response.Content.ReadAsStringAsync();
        return Json(new { success = false, message = $"Lỗi từ Google API: {errorContent}" });
      }

      // 4. Đọc dữ liệu JSON trả về từ Google
      string jsonResponse = await response.Content.ReadAsStringAsync();

      // Trích xuất chuỗi chữ thô từ JSON của Google bằng JsonDocument (nhanh, không cần tạo class mapping)
      using var doc = JsonDocument.Parse(jsonResponse);
      string extractedText = "";

      var root = doc.RootElement;
      if (root.TryGetProperty("responses", out var responses) && responses.GetArrayLength() > 0)
      {
        var firstResponse = responses[0];
        if (firstResponse.TryGetProperty("textAnnotations", out var textAnnotations) && textAnnotations.GetArrayLength() > 0)
        {
          // Phần tử đầu tiên trong textAnnotations luôn chứa toàn bộ đoạn văn bản quét được
          extractedText = textAnnotations[0].GetProperty("description").GetString();
        }
      }

      // 5. Bóc tách dữ liệu bằng Regex
      var result = ParseCccdData(extractedText);

      return Json(new { success = true, data = result, rawText = extractedText });
    }
    catch (Exception ex)
    {
      return Json(new { success = false, message = "Lỗi xử lý OCR: " + ex.Message });
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

  private List<Dictionary<string, object>> ConvertDynamicToDictionary(
      List<dynamic> rows)
  {
    return rows.Select(r =>
        ((IDictionary<string, object>)r)
        .ToDictionary(x => x.Key, x => x.Value)
    ).ToList();
  }
  public class CccdResult
  {
    public string IdNumber { get; set; }
    public string FullName { get; set; }
    public string Gender { get; set; }
  }
}
