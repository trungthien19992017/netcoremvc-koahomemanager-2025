
using Microsoft.EntityFrameworkCore;
using KOAHome.EntityFramework;
using Microsoft.Data.SqlClient;
using System.Dynamic;
using System.Text;
using System.Data;

namespace KOAHome.Services
{
  public interface IWidgetService
  {
    public Task<IDictionary<string, object>> Widget_GetObject(Dictionary<string, object> parameters, string sqlStore, string? connectionString);
    public Task<List<dynamic>> Widget_GetList(Dictionary<string, object> parameters, string sqlStore, string? connectionString);
  }
  public class WidgetService : IWidgetService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    public WidgetService(QLKCL_NEWContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con)
    {
      _db = db;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
      _con = con;
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

  }
}
