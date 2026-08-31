
using KOAHome.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Text.Json.Nodes;

namespace KOAHome.Services
{
  public interface IWidgetService
  {
    public Task<IDictionary<string, object>> Widget_GetObject(Dictionary<string, object> parameters, string sqlStore, string? connectionString);
    public Task<List<dynamic>> Widget_GetList(Dictionary<string, object> parameters, string sqlStore, string? connectionString);
    public Task<JsonObject> Widget_GetDashboardData(string dashboardCode, long userId, JsonObject preview = null, JsonObject filters = null);
  }
  public class WidgetService : IWidgetService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    private readonly IMemoryCache _cache;
    public WidgetService(QLKCL_NEWContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con, IMemoryCache cache)
    {
      _db = db;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
      _con = con;
      _cache = cache;
    }

    public async Task<IDictionary<string, object>> Widget_GetObject(Dictionary<string, object> parameters, string sqlStore, string? connectionString)
    {
      // neu khong truyen connect string thi se lay connection string mac dinh
      if (connectionString == null)
      {
        connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      }

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      // Chuyển đổi List<dynamic> thành Dictionary<string, object>
      var dictionary = resultList
    .SelectMany(obj => ((IDictionary<string, object>)obj)
        .Select(prop => new KeyValuePair<string, object>(prop.Key, prop.Value)))
    .ToDictionary(pair => pair.Key, pair => pair.Value);

      // nhan du lieu duoi dang object
      return dictionary;
    }

    public async Task<List<dynamic>> Widget_GetList(Dictionary<string, object> parameters, string sqlStore, string? connectionString)
    {
      // neu khong truyen connect string thi se lay connection string mac dinh
      if (connectionString == null)
      {
        connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      }
      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return resultList;
    }

    public async Task<JsonObject> Widget_GetDashboardData(string dashboardCode, long userId, JsonObject preview = null, JsonObject filters = null)
    {
      string connectionString = _configuration.GetConnectionString("ConfigConnection");
      string sqlStore = "net_dashboard_get_all_widgets2";
      var parameters = new Dictionary<string, object>
      {
        ["dashboardcode"] = dashboardCode,
        ["userid"] = userId,
        ["previewwidget"] = (object)preview?.ToJsonString() ?? DBNull.Value,
        ["filters"] = filters?.ToJsonString() ?? "{}"
      };
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var result = await _con.Connection_GetSingleDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      var data = result.ContainsKey("result_json") ? Convert.ToString(result["result_json"]) ?? "{}" : "{}";

      return data == null ? null : JsonNode.Parse(data.ToString()) as JsonObject;
    }
  }
}
