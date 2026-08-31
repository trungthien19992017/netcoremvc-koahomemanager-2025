using KOAHome.EntityFramework;
using KOAHome.Models;
using Microsoft.Extensions.Options;
using System.Text.Json.Nodes;

namespace KOAHome.Services
{
  public interface IDashboardService
  {
    //public Task<object> SaveDashboardTable(IFormCollection form, int Id);
    public Task<IDictionary<string, object>?> NET_DashboardConfig_Get(string dashboardCode);
    public Task<IDictionary<string, object>?> NET_WidgetConfig_Get(string dashboardCode);
    public Task<List<dynamic>> NET_DashboardConfig_ups(Dictionary<string, object> parameters, string? dashboardCode, string sqlStore, string? connectionString);
    public Task<JsonObject> NET_DashboardConfig_Get2(string dashboardCode);
    public Task<List<dynamic>> NET_DashboardConfig_ups2(string code, long userId, JsonObject options, JsonArray widgets);
    public Task ValidateWidget(JsonObject widget);
    public Task<JsonObject> Normalize(JsonObject widget, JsonObject source);

  }
  public partial class DashboardService : IDashboardService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    private const string CartSession = "CartSession";
    private readonly CloudflareR2Config _r2config;
    public DashboardService(QLKCL_NEWContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con, IOptions<CloudflareR2Config> r2config)
    {
      _db = db;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
      _con = con;
      _r2config = r2config.Value;
    }
    //public async Task<object> SaveDashboardTable(IFormCollection form, int Id)
    //{
    //  if (form.Files.Any())
    //  {
    //    // chuyen thong tin file vao paramerter
    //    // Lấy danh sách tên file
    //    var fileInfos = form.Files.Select(f => new
    //    {
    //      SyntaxCode = f.Name,
    //      FileName = f.FileName,
    //      ContentType = f.ContentType
    //    }).ToList();

    //    // Chuyển danh sách thành chuỗi JSON
    //    string fileInfosJson = JsonConvert.SerializeObject(fileInfos);

    //    // Dictionary chứa các tham số
    //    var parameters = new Dictionary<string, object>
    //  {
    //      { "id", Id},
    //      { "fileinfosjson", fileInfosJson ?? (object)DBNull.Value }
    //  };


    //    var connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
    //    //var sqlQuery = "EXEC dbo.HS_Customer_Search @Param1";
    //    string sqlStore = "net_Dashboard_savefile";

    //    // chuyen thanh cau query tu store va param truyen vao
    //    var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

    //    var resultList = new List<dynamic>();

    //    // xu ly lay du lieu dua truyen store va param truyen vao
    //    resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

    //    //kiem tra du lieu id tra ve
    //    var ids_return = resultList
    //    .Where(item => ((IDictionary<string, object>)item).ContainsKey("id"))
    //    .Select(item => ((IDictionary<string, object>)item)["id"])
    //    .FirstOrDefault(); // Lọc ra những phần tử có Id

    //    // neu co gia tri tra ve thi bao thanh cong
    //    if (ids_return != null)
    //    {
    //      string listidStr = ids_return.ToString();

    //      if (!string.IsNullOrWhiteSpace(listidStr))
    //      {
    //        // Trả về kiểu object để controller serialize thành JsonResult
    //        return new
    //        {
    //          success = true,
    //          listDashboardId = listidStr
    //        };
    //      }
    //    }
    //    return new { success = false, errorMessage = "Lưu file không thành công" };
    //  }
    //  // Trả về kiểu object để controller serialize thành JsonResult
    //  return new
    //  {
    //    success = true
    //  };
    //}
    public async Task<IDictionary<string, object>?> NET_DashboardConfig_Get(string dashboardCode)
    {
      // su dung datasource config de lay du lieu
      string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      // store get du lieu
      string sqlStore = "net_dashboard_sel";
      // khai bao param lien quan
      var parameters = new Dictionary<string, object>();
      parameters.Add("dashboardcode", dashboardCode);

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      // xu ly lay du lieu dua truyen store va param truyen vao
      var result = await _con.Connection_GetSingleDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return result;
    }

    public async Task<IDictionary<string, object>?> NET_WidgetConfig_Get(string dashboardCode)
    {
      // su dung datasource config de lay du lieu
      string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      // store get du lieu
      string sqlStore = "net_widget_sel_by_dashboard";
      // khai bao param lien quan
      var parameters = new Dictionary<string, object>();
      parameters.Add("dashboardcode", dashboardCode);

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      // xu ly lay du lieu dua truyen store va param truyen vao
      var result = await _con.Connection_GetSingleDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return result;
    }

    public async Task<List<dynamic>> NET_DashboardConfig_ups(Dictionary<string, object> parameters, string? dashboardCode, string sqlStore, string? connectionString)
    {
      // add Id vao paramerter neu co
      if (dashboardCode != null && !parameters.ContainsKey("dashboardcode"))
      {
        parameters.Add("dashboardcode", dashboardCode);
      }
      // neu khong truyen connect string thi se lay connection string mac dinh
      if (connectionString == null)
      {
        connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      }
      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return resultList;
    }

    public async Task<JsonObject> NET_DashboardConfig_Get2(string dashboardCode)
    {
      string connectionString = _configuration.GetConnectionString("ConfigConnection");
      string sqlStore = "net_dashboard_sel2";
      var parameters = new Dictionary<string, object>();
      parameters.Add("dashboardcode", dashboardCode);
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var result = await _con.Connection_GetSingleDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      var data = result.ContainsKey("data") ? Convert.ToString(result["data"]) ?? "{}" : "{}";

      return data == null ? null : JsonNode.Parse(data.ToString()) as JsonObject;
    }

    public async Task ValidateWidget(JsonObject widget)
    {
      var kind = DashboardMapper.Text(widget["kind"]);
      if (!DashboardMapper.Kinds.Contains(kind)) throw new FormatException("Loại widget không hỗ trợ.");
      var config = widget["config"] as JsonObject ?? throw new FormatException("Thiếu cấu hình widget.");
      var code = DashboardMapper.Text(config["code"]);
      if (code is "__proto__" or "constructor" or "prototype" || !System.Text.RegularExpressions.Regex.IsMatch(code, "^[a-zA-Z0-9_-]{1,160}$")) throw new FormatException("Mã widget chỉ gồm chữ, số, dấu gạch ngang/gạch dưới; tối đa 160 ký tự và không dùng tên hệ thống.");
      if (config["dataBinding"] is not JsonObject binding)
      {
        if (kind is "button" or "emoji_card") return;
        throw new FormatException("Widget chưa có cấu hình nguồn dữ liệu.");
      }
      if (DashboardMapper.Text(binding["version"]) != "1" || DashboardMapper.Text(binding["type"]) is not ("store" or "sqlcontent" or "json")) throw new FormatException("Loại nguồn hoặc phiên bản cấu hình không hợp lệ.");
      var content = DashboardMapper.Text(binding["content"]);
      if (string.IsNullOrWhiteSpace(content) || System.Text.Encoding.UTF8.GetByteCount(content) > 262144) throw new FormatException("Nội dung nguồn không được rỗng hoặc vượt 256 KB.");
      if (binding["parameters"] != null && binding["parameters"] is not JsonArray) throw new FormatException("Tham số phải là mảng JSON.");
      if (binding["mapping"] != null && binding["mapping"] is not JsonObject) throw new FormatException("Ánh xạ phải là object JSON.");
      if (DashboardMapper.Text(binding["type"]) == "json") JsonNode.Parse(content);
    }
    public async Task<JsonObject> Normalize(JsonObject widget, JsonObject source)
    {
      var config = (JsonObject)widget["config"];
      var result = new JsonObject { ["label"] = DashboardMapper.Text(config["title"]), ["shape"] = source?["shape"]?.DeepClone(), ["data"] = new JsonObject() };
      if (source == null || source["error"] != null)
      {
        result["status"] = "error"; result["notice"] = source?["error"]?.DeepClone() ?? JsonValue.Create("Không nhận được kết quả từ nguồn."); return result;
      }
      try
      {
        var kind = DashboardMapper.Text(widget["kind"]);
        var data = config["dataBinding"] is JsonObject binding ? DashboardMapper.Map(kind, binding, source["data"]) : new JsonObject();
        result["data"] = data;
        result["status"] = DashboardMapper.Empty(kind, data) ? "empty" : "ready";
        result["notice"] = DashboardMapper.Empty(kind, data) ? "Nguồn không có dữ liệu hiển thị." : "";
      }
      catch (Exception ex) when (ex is FormatException or InvalidOperationException or System.Text.Json.JsonException or OverflowException)
      {
        result["status"] = "error"; result["notice"] = "Ánh xạ chưa hợp lệ: " + ex.Message;
      }
      return result;
    }

    public async Task<List<dynamic>> NET_DashboardConfig_ups2(string code, long userId, JsonObject options, JsonArray widgets)
    {
      string connectionString = _configuration.GetConnectionString("ConfigConnection");
      string sqlStore = "net_dashboard_ups2";
      var parameters = new Dictionary<string, object>()
      {
        ["dashboardcode"] = code,
        ["userid"] = userId,
        ["options"] = options.ToJsonString(),
        ["widgets"] = widgets.ToJsonString()
      };
      // neu khong truyen connect string thi se lay connection string mac dinh
      if (connectionString == null)
      {
        connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      }
      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return resultList;
    }
  }
}
