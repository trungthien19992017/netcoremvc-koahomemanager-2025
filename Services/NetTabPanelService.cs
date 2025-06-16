using KOAHome.EntityFramework;

namespace KOAHome.Services
{
  public interface INetTabPanelService
  {
    public Task<IDictionary<string, object>?> NET_TabPanel_Get(string tabCode);
    public Task<List<dynamic>> NET_TabPanel_Detail_Get(string? tabCode, int? tabId);
  }
  public class NetTabPanelService : INetTabPanelService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly TttConfigContext _dbconfig;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    public NetTabPanelService(QLKCL_NEWContext db, TttConfigContext dbconfig, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con)
    {
      _db = db;
      _dbconfig = dbconfig;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
      _con = con;
    }

    public async Task<IDictionary<string, object>?> NET_TabPanel_Get(string tabCode)
    {
      // su dung datasource config de lay du lieu
      string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      // store get du lieu
      string sqlStore = "NET_TabPanel_sel";
      // khai bao param lien quan
      var parameters = new Dictionary<string, object>();
      parameters.Add("tabcode", tabCode);

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      // xu ly lay du lieu dua truyen store va param truyen vao
      var result = await _con.Connection_GetSingleDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return result;
    }
    public async Task<List<dynamic>> NET_TabPanel_Detail_Get(string? tabCode, int? tabId)
    {
      // su dung datasource config de lay du lieu
      string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      // store get du lieu
      string sqlStore = "NET_TabPanel_Detail_sel";
      // khai bao param lien quan
      var parameters = new Dictionary<string, object>();
      parameters.Add("tabcode", tabCode);
      parameters.Add("tabid", tabId);

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return resultList;
    }
  }
}
