
using Microsoft.EntityFrameworkCore;
using KOAHome.EntityFramework;
using Microsoft.Data.SqlClient;
using System.Dynamic;
using System.Text;
using System.Data;

namespace KOAHome.Services
{
  public interface IActionService
  {
    public Task<List<dynamic>> Action_store(Dictionary<string, object> parameters, string sqlStore, string? connectionString);
    public Task<List<dynamic>> NET_ActionListDetail_WithObject_Get(string? objectCode, int? objectId, string actionListTypeCode);
  }
  public class ActionService : IActionService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    public ActionService(QLKCL_NEWContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con)
    {
      _db = db;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
      _con = con;
    }

    public async Task<List<dynamic>> Action_store(Dictionary<string, object> parameters, string sqlStore, string? connectionString)
    {
      if (parameters == null)
      {
        parameters = new Dictionary<string, object>();
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

    public async Task<List<dynamic>> NET_ActionListDetail_WithObject_Get(string? objectCode, int? objectId, string actionListTypeCode)
    {
      // su dung datasource config de lay du lieu
      string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      // store get du lieu
      string sqlStore = "net_actionlistdetail_withobject_sel";
      // khai bao param lien quan
      var parameters = new Dictionary<string, object>();
      parameters.Add("objectcode", objectCode);
      parameters.Add("objectid", objectId);
      parameters.Add("actionlisttypecode", actionListTypeCode);

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return resultList;
    }
  }
}
