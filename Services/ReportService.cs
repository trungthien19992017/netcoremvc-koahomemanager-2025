
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
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;
using System.Data;
using System.Security.Cryptography;

namespace KOAHome.Services
{
  public interface IReportService
  {
    public Task<List<dynamic>> Report_search(Dictionary<string, object> parameters, string sqlStore, string? connectionString);
    public Task<(List<dynamic> Results, int TotalRecord, int MaxPage, int TotalPage)> Report_Pagination_search(string sqlStore, string? connectionString, Dictionary<string, object> parameters, int page = 1, int pageSize = 10);
    public Task<List<dynamic>> ReportDetail_FromParent(string parentKey, string parentValue, string sqlStore, string? connectionString);
  }
  public class ReportService : IReportService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    public ReportService(QLKCL_NEWContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con)
    {
      _db = db;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
      _con = con;
    }
    public async Task<List<dynamic>> Report_search(Dictionary<string, object> parameters, string sqlStore, string? connectionString)
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

    public async Task<(List<dynamic> Results, int TotalRecord, int MaxPage, int TotalPage)> Report_Pagination_search(string sqlStore, string? connectionString, Dictionary<string, object> parameters, int page = 1, int pageSize = 20)
    {
      // neu khong truyen connect string thi se lay connection string mac dinh
      if (connectionString == null)
      {
        connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      }
      //var sqlQuery = new StringBuilder("EXEC dbo." + sqlStore);

      //var sqlParams = new List<SqlParameter>();

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();
      var totalRecord = 0;
      var maxPage = 0;
      var totalPage = 0;
      
      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      totalRecord = resultList.Count();
      maxPage = 3;
      if (totalRecord % pageSize != 0)
        totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize)) + 1;
      else
        totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));


      //Phân trang
      if (resultList.Skip((page - 1) * pageSize).Count() < pageSize)
        resultList = resultList.Skip((page - 1) * pageSize).Take((int)(totalRecord % pageSize)).ToList();
      else
        resultList = resultList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
      return (resultList, totalRecord, maxPage, totalPage);
    }

    public async Task<List<dynamic>> ReportDetail_FromParent(string parentKey, string parentValue, string sqlStore, string? connectionString)
    {
      // neu khong truyen connect string thi se lay connection string mac dinh
      if (connectionString == null)
      {
        connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      }
      // Dictionary chứa các tham số
      var parameters = new Dictionary<string, object>
      {
          { parentKey, parentValue }
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
