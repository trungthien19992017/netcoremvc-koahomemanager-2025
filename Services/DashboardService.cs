using KOAHome.EntityFramework;
using KOAHome.Models;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Newtonsoft.Json;
using System.Data;

namespace KOAHome.Services
{
  public interface IDashboardService
  {
    //public Task<object> SaveDashboardTable(IFormCollection form, int Id);
    public Task<IDictionary<string, object>?> NET_DashboardConfig_Get(string dashboardCode);
    public Task<IDictionary<string, object>?> NET_WidgetConfig_Get(string dashboardCode);
    public Task<List<dynamic>> NET_DashboardConfig_ups(Dictionary<string, object> parameters, string? dashboardCode, string sqlStore, string? connectionString);

  }
  public class DashboardService : IDashboardService
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

  }
}
