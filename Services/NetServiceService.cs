
using Microsoft.EntityFrameworkCore;
using KOAHome.EntityFramework;
using Microsoft.Data.SqlClient;
using System.Dynamic;
using System.Text;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace KOAHome.Services
{
  public interface INetServiceService
  {
    public Task<List<SelectListItem>> NET_Service_DynamicExecute(int serviceId, Dictionary<string, object>? parameters);
  }
  public class NetServiceService : INetServiceService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    public NetServiceService(QLKCL_NEWContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con)
    {
      _db = db;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
      _con = con;
    }

    public async Task<List<SelectListItem>> NET_Service_DynamicExecute(int serviceId, Dictionary<string, object>? parameters)
    {
        // su dung datasource config de lay du lieu
        string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
                                                                                          // store get du lieu
        string sqlStore = "NET_Service_DynamicExecute";
        // neu parameter rong thi tu tao 1 parameter moi truyen vao
        if (parameters == null)
        {
          parameters = new Dictionary<string, object>();
        }

        // chuyen tat ca param dang co thanh 1 chuoi json va truyen vao bien Param
        var displayParameter = new Dictionary<string, object>();
        if (!parameters.ContainsKey("Param"))
        {
            parameters.Add("Param", JsonConvert.SerializeObject(parameters));
        }
        parameters.Add("ServiceId", serviceId);

        // chuyen thanh cau query tu store va param truyen vao
        var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

        var resultList = new List<dynamic>();

        // xu ly lay du lieu dua truyen store va param truyen vao
        resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      List<SelectListItem> listItems = resultList
          .Select(x => new SelectListItem
          {
            Value = x.Id.ToString(),    // hoặc x.GetType().GetProperty("Id") nếu không chắc
            Text = x.Name.ToString()
          })
          .ToList();

      return listItems;
    }

  }
}
