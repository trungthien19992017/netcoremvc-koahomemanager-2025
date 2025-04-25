
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
    public Task<IDictionary<string, object>> Form_sel(Dictionary<string, object>? parameters, int? Id, string sqlStore, string? connectionString);
    public Task<List<dynamic>> Form_ups(Dictionary<string, object> parameters, int? id, string sqlStore, string? connectionString);
    public Task<IDictionary<string, object>> Form_GetDataFill_FromSelection(Dictionary<string, object> parameters, string sqlStore, string? connectionString);
    // xu ly cau hinh form
    public Task<int?> GetFormIdFromCode(string formCode);
    public Task<IDictionary<string, object>?> NET_Form_Get(string formCode);
    // lay thong tin form version field
    public Task<List<dynamic>> NET_Form_VersionField_WithForm_sel(string? formCode, int? formId);

  }
  public class FormService : IFormService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    private readonly TttConfigContext _dbconfig;

    public FormService(QLKCL_NEWContext db, TttConfigContext dbconfig, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con)
    {
      _db = db;
      _dbconfig = dbconfig;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
      _con = con;
    }

    public async Task<IDictionary<string, object>> Form_sel(Dictionary<string, object>? parameters, int? Id, string sqlStore, string? connectionString)
    {
      // neu khong truyen connect string thi se lay connection string mac dinh
      if (connectionString == null)
      {
        connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      }
      if (parameters == null)
      {
        parameters = new Dictionary<string, object>();
      }

      // neu chua ton tai Id them them vao
      if (!parameters.ContainsKey("Id"))
      {
        parameters.Add("Id", Id);
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

    public async Task<IDictionary<string, object>> Form_GetDataFill_FromSelection(Dictionary<string, object> parameters, string sqlStore, string? connectionString)
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
    public async Task<int?> GetFormIdFromCode(string formCode)
    {
      int? reportId = null;
      reportId = await _dbconfig.NetForms.Where(p => p.Code == formCode).Select(p => (int?)p.Id).FirstOrDefaultAsync();

      return reportId;
    }
    public async Task<IDictionary<string, object>?> NET_Form_Get(string formCode)
    {
      // su dung datasource config de lay du lieu
      string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      // store get du lieu
      string sqlStore = "NET_Form_sel";
      // khai bao param lien quan
      var parameters = new Dictionary<string, object>();
      parameters.Add("FormCode", formCode);

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      // xu ly lay du lieu dua truyen store va param truyen vao
      var result = await _con.Connection_GetSingleDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return result;
    }
    public async Task<List<dynamic>> NET_Form_VersionField_WithForm_sel(string? formCode, int? formId)
    {
      // su dung datasource config de lay du lieu
      string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      // store get du lieu
      string sqlStore = "NET_Form_VersionField_WithForm_sel";
      // khai bao param lien quan
      var parameters = new Dictionary<string, object>();
      parameters.Add("FormCode", formCode);
      parameters.Add("FormId", formId);

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return resultList;
    }

  }
}
