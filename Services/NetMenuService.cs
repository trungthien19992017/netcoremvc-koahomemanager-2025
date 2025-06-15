
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
using System.Text.RegularExpressions;
using OfficeOpenXml;
using System.Reflection.Metadata;

namespace KOAHome.Services
{
  public interface INetMenuService
  {
    public Task<List<dynamic>> NET_MainMenu_List(Dictionary<string, object>? parameters);
    public Task<IDictionary<string, object>?> NET_MainMenu_Get(string menuCode);
    public Task<List<dynamic>> NET_Menu_WithMainMenu_Get(string? menuCode, int? menuId);
  }
  public class NetMenuService : INetMenuService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly TttConfigContext _dbconfig;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    public NetMenuService(QLKCL_NEWContext db, TttConfigContext dbconfig, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con)
    {
      _db = db;
      _dbconfig = dbconfig;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
      _con = con;
    }

    public async Task<List<dynamic>> NET_MainMenu_List(Dictionary<string, object>? parameters)
    {
      // su dung datasource config de lay du lieu
      string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      // store get du lieu
      string sqlStore = "NET_MainMenu_search";

      // neu chưa tồn tại parameter thì tạo param ảo
      if (parameters == null)
      {
        parameters = new Dictionary<string, object>();
      }

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return resultList;
    }

    public async Task<IDictionary<string, object>?> NET_MainMenu_Get(string menuCode)
    {
      // su dung datasource config de lay du lieu
      string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      // store get du lieu
      string sqlStore = "NET_MainMenu_sel";
      // khai bao param lien quan
      var parameters = new Dictionary<string, object>();
      parameters.Add("menucode", menuCode);

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      // xu ly lay du lieu dua truyen store va param truyen vao
      var result = await _con.Connection_GetSingleDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return result;
    }
    public async Task<List<dynamic>> NET_Menu_WithMainMenu_Get(string? menuCode, int? menuId)
    {
      // su dung datasource config de lay du lieu
      string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      // store get du lieu
      string sqlStore = "NET_Menu_WithMainMenu_sel";
      // khai bao param lien quan
      var parameters = new Dictionary<string, object>();
      parameters.Add("menucode", menuCode);
      parameters.Add("menuid", menuId);

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return resultList;
    }
  }
}
