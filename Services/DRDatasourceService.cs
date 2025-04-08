
using Microsoft.EntityFrameworkCore;
using KOAHome.EntityFramework;
using Microsoft.Data.SqlClient;
using System.Dynamic;
using System.Text;
using System.Data;
using Microsoft.Extensions.Hosting;
using System.Xml.Linq;

namespace KOAHome.Services
{
  public interface IDRDatasourceService
  {
    public Task<string> GetConnectionString(int datasourceId);
  }
  public class DRDatasourceService : IDRDatasourceService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    public DRDatasourceService(QLKCL_NEWContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con)
    {
      _db = db;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
      _con = con;
    }

    public async Task<string> GetConnectionString(int datasourceId)
    {
      // su dung datasource config de lay du lieu
      string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      // store get du lieu
      string sqlStore = "NET_DatasourceDetail_sel";
      // khai bao param lien quan
      var parameters = new Dictionary<string, object>();
      parameters.Add("Id", datasourceId);

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      // xu ly lay du lieu dua truyen store va param truyen vao
      var result = await _con.Connection_GetSingleDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      // tra ve chuoi connection string
      string resultConnectionString = $"Server={result.Host};Database={result.DBName};User Id={result.User ?? ""};Password={result.Password ?? ""};Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;Max Pool Size=100;";

      return resultConnectionString;
    }

  }
}
