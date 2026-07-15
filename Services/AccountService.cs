
using KOAHome.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace KOAHome.Services
{
  public interface IAccountService
  {
    public Task<IDictionary<string, object>?> NET_User_Get(int userId);
  }
  public class AccountService : IAccountService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly TttConfigContext _dbconfig;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    public AccountService(QLKCL_NEWContext db, TttConfigContext dbconfig, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con)
    {
      _db = db;
      _dbconfig = dbconfig;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
      _con = con;
    }
    public async Task<IDictionary<string, object>?> NET_User_Get(int userId)
    {
      string connectionString = _configuration.GetConnectionString("ConfigConnection");
      string sqlStore = "NET_User_sel";
      var parameters = new Dictionary<string, object>();
      parameters.Add("userid", userId);

      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var result = await _con.Connection_GetSingleDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return result;
    }
  }
}
