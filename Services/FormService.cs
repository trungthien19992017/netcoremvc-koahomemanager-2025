
using Microsoft.EntityFrameworkCore;
using KOAHome.EntityFramework;
using Microsoft.Data.SqlClient;
using System.Dynamic;
using System.Text;
using System.Data;

namespace KOAHome.Services
{
  public interface IFormService
  {
    public Task<IDictionary<string, object>> Form_sel(int? Id, string sqlStore, string? connectionString);
    public Task<List<dynamic>> Form_ups(Dictionary<string, object> parameters, int? id, string sqlStore, string? connectionString);
    public Task<IDictionary<string, object>> Form_GetDataFill_FromSelection(Dictionary<string, object> parameters, string sqlStore, string? connectionString);

  }
  public class FormService : IFormService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    public FormService(QLKCL_NEWContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con)
    {
      _db = db;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
      _con = con;
    }

    public async Task<IDictionary<string, object>> Form_sel(int? Id, string sqlStore, string? connectionString)
    {
      // neu khong truyen connect string thi se lay connection string mac dinh
      if (connectionString == null)
      {
        connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      }
      // Dictionary chứa các tham số
      var parameters = new Dictionary<string, object>
      {
          { "Id", Id}
      };

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

    public async Task<List<dynamic>> Form_ups(Dictionary<string, object> parameters, int? id, string sqlStore, string? connectionString)
    {
      // add Id vao paramerter neu co
      if (id != null && !parameters.ContainsKey("Id"))
      {
        parameters.Add("Id", id);
      }
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

    public async Task<IDictionary<string, object>> Form_GetDataFill_FromSelection(Dictionary<string, object> parameters,string sqlStore, string? connectionString)
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

      return dictionary;
    }

  }
}
