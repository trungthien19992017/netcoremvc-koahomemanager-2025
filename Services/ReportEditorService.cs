
using KOAHome.EntityFramework;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace KOAHome.Services
{
  public interface IReportEditorService
  {
    public Task<string> ExtractGridDataToJson(IFormCollection form);
    public Task<List<dynamic>> ReportEditor_Json_Update(int? Id, string json, string sqlStore, string? connectionString);

  }
  public class ReportEditorService : IReportEditorService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    private const string CartSession = "CartSession";
    public ReportEditorService(QLKCL_NEWContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con)
    {
      _db = db;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
      _con = con;
    }
    public async Task<string> ExtractGridDataToJson(IFormCollection form)
    {
      // Dictionary để nhóm dữ liệu theo số thứ tự [n]
      var gridData = new Dictionary<int, Dictionary<string, string>>();

      foreach (var key in form.Keys)
      {
        var match = Regex.Match(key, @"grid\[(\d+)\]\.(\w+)");
        if (match.Success)
        {
          int index = int.Parse(match.Groups[1].Value); // Lấy số thứ tự n
          string field = match.Groups[2].Value; // Lấy tên cột (Id, Quantity, Description,...)
          string value = form[key].ToString();

          if (!gridData.ContainsKey(index))
          {
            gridData[index] = new Dictionary<string, string>();
          }

          // Nếu giá trị là chuỗi rỗng, gán `null`
          if (string.IsNullOrEmpty(value))
          {
            value = null;
          }
          else 
          // Kiểm tra nếu field chứa "date" thì format thành yyyy-MM-dd HH:mm:ss
          if (field.IndexOf("date", StringComparison.OrdinalIgnoreCase) >= 0)
          {
            if (DateTime.TryParse(value, out DateTime parsedDate))
            {
              value = parsedDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
          }

          gridData[index][field] = value; // Gán giá trị vào dictionary
        }
      }

      // Chuyển đổi dữ liệu sang JSON
      return await Task.FromResult(JsonConvert.SerializeObject(gridData.Values, Formatting.Indented));
    }

    public async Task<List<dynamic>> ReportEditor_Json_Update(int? Id, string json, string sqlStore, string? connectionString)
    {
      // neu khong truyen connect string thi se lay connection string mac dinh
      if (connectionString == null)
      {
        connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      }

      // Dictionary chứa các tham số
      var parameters = new Dictionary<string, object>
      {
          { "Id", Id ?? (object)DBNull.Value },
          { "json", string.IsNullOrEmpty(json) ? (object)DBNull.Value : json }
      };

      //voi du lieu param lay tu store, kiem tra dinh dang de xu ly
      // kiem tra du lieu nhap vao co trong store thi dua vao lenh xu ly
      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);
      return resultList;
    }

  }
}
