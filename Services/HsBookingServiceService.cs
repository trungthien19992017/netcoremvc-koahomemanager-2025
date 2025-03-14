
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using KOAHome.EntityFramework;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Dynamic;
using System.Text;
using System.Data;

namespace KOAHome.Services
{
  public interface IHsBookingServiceService
  {
    public Task<List<HsBookingService>> LoadListBookingService();
    public Task<List<dynamic>> HS_BookingService_Info(Dictionary<string, object> parameters);
    public Task<List<dynamic>> HS_BookingService_FromBooking(int BookingID);
    public Task<List<dynamic>> HS_BookingService_Json_Update(int? Id, string json);

  }
  public class HsBookingServiceService : IHsBookingServiceService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    private const string CartSession = "CartSession";
    public HsBookingServiceService(QLKCL_NEWContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con)
    {
      _db = db;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
      _con = con;
    }
    public async Task<List<HsBookingService>> LoadListBookingService()
    {
      return await _db.HsBookingServices.ToListAsync();
    }
    public async Task<List<dynamic>> HS_BookingService_Info(Dictionary<string, object> parameters)
    {
      var connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      var sqlStore = "HS_BookingService_Info";

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);
      return resultList;
    }

    public async Task<List<dynamic>> HS_BookingService_FromBooking(int BookingID)
    {
      var connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      var sqlStore = "HS_BookingService_search";

      // Dictionary chứa các tham số
      var parameters = new Dictionary<string, object>
      {
          { "BookingID", BookingID }
      };

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);
      return resultList;
    }

    public async Task<List<dynamic>> HS_BookingService_Json_Update(int? Id, string json)
    {
      var connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      //var sqlQuery = "EXEC dbo.HS_Customer_Search @Param1";
      string sqlStore = "HS_BookingService_Json_ups";

      // Dictionary chứa các tham số
      var parameters = new Dictionary<string, object>
      {
          { "Id", Id ?? (object)DBNull.Value },
          { "json", string.IsNullOrEmpty(json) ? (object)DBNull.Value : json }
      };

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return resultList;
    }

  }
}
