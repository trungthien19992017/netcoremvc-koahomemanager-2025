
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

namespace KOAHome.Services
{
  public interface IReportService
  {
    public Task<List<dynamic>> Report_search(Dictionary<string, object> parameters, string sqlStore, string? connectionString);
    public Task<(List<dynamic> Results, int TotalRecord, int MaxPage, int TotalPage)> Report_Pagination_search(string sqlStore, string? connectionString, Dictionary<string, object> parameters, int page = 1, int pageSize = 10);
    public Task<List<dynamic>> ReportDetail_FromParent(string parentKey, string parentValue, string sqlStore, string? connectionString);
    public Task<string> ExtractImportDataToJson(IFormFile file);
    public Task<string> ExtractImportDataToStoreName(IFormFile file);
    public Task<List<dynamic>> Import_Json_Update(int? Id, string json, string sqlStore, string? connectionString);
    public Task<dynamic> NET_Report_Get(string reportCode);
    public Task<List<dynamic>> NET_Filter_WithReport_Get(string? reportCode, int? reportId);
    public Task<List<dynamic>> NET_Display_WithReport_Get(string? reportCode, int? reportId);
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

    // tach du lieu import thanh chuoi json
    public async Task<string> ExtractImportDataToJson(IFormFile file)
    {
      var result = new Dictionary<int, Dictionary<string, string>>();

      string sqlstore = "";

      // Đọc file Excel
      using (var stream = new MemoryStream())
      {
        await file.CopyToAsync(stream);
        using (var package = new ExcelPackage(stream))
        {
          // Sheet "Import" để lấy tên Store Procedure
          var importSheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Import");
          if (importSheet != null)
          {
            sqlstore = importSheet.Cells[1, 1].Text?.Trim(); // Lấy A1
          }

          ExcelWorksheet worksheet = package.Workbook.Worksheets.First();
          int rowCount = worksheet.Dimension.Rows;
          int colCount = worksheet.Dimension.Columns;

          // Lấy tên cột từ dòng thứ 2
          var columnNames = new List<string>();
          for (int col = 1; col <= colCount; col++)
          {
            var colName = worksheet.Cells[2, col].Text.Trim();
            columnNames.Add(colName);
          }

          // Duyệt từng dòng dữ liệu từ dòng 3
          for (int row = 3; row <= rowCount; row++)
          {
            bool hasData = false;

            // Kiểm tra nếu dòng này có dữ liệu không
            for (int col = 1; col <= colCount; col++)
            {
              var value = worksheet.Cells[row, col].Value;
              if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
              {
                hasData = true;
                break;
              }
            }

            if (!hasData) continue; // Nếu dòng trống thì bỏ qua

            var rowData = new Dictionary<string, string>();

            // Lưu trữ các giá trị vào Dictionary theo tên cột
            for (int col = 1; col <= colCount; col++)
            {
              var key = columnNames[col - 1];
              var value = worksheet.Cells[row, col].Value?.ToString().Trim() ?? string.Empty;
              rowData[key] = value;
            }

            result[row - 2] = rowData; // Lưu dữ liệu theo chỉ số dòng (dòng 3 là rowIndex = 1)
          }
        }
      }


      // Chuyển đổi dữ liệu sang JSON
      return await Task.FromResult(JsonConvert.SerializeObject(result.Values, Formatting.Indented));
    }

    // lay ten store procedure tu import file
    public async Task<string> ExtractImportDataToStoreName(IFormFile file)
    {
      string sqlstore = "";

      // Đọc file Excel
      using (var stream = new MemoryStream())
      {
        await file.CopyToAsync(stream);
        using (var package = new ExcelPackage(stream))
        {
          // Sheet "Import" để lấy tên Store Procedure
          var importSheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Import");
          if (importSheet != null)
          {
            sqlstore = importSheet.Cells[1, 1].Text?.Trim() ?? ""; // Lấy A1
          }
        }
      }

      // Chuyển đổi dữ liệu sang JSON
      return sqlstore;
    }

    // nhap file tu chuoi json khi import
    public async Task<List<dynamic>> Import_Json_Update(int? Id, string json, string sqlStore, string? connectionString)
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
    public async Task<dynamic> NET_Report_Get(string reportCode)
    {
      // su dung datasource config de lay du lieu
      string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      // store get du lieu
      string sqlStore = "NET_Report_sel";
      // khai bao param lien quan
      var parameters = new Dictionary<string, object>();
      parameters.Add("ReportCode", reportCode);

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      // xu ly lay du lieu dua truyen store va param truyen vao
      var result = await _con.Connection_GetSingleDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return result;
    }
    public async Task<List<dynamic>> NET_Filter_WithReport_Get(string? reportCode, int? reportId)
    {
      // su dung datasource config de lay du lieu
      string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      // store get du lieu
      string sqlStore = "NET_Filter_WithReport_sel";
      // khai bao param lien quan
      var parameters = new Dictionary<string, object>();
      parameters.Add("ReportCode", reportCode);
      parameters.Add("ReportId", reportId);

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return resultList;
    }
    public async Task<List<dynamic>> NET_Display_WithReport_Get(string? reportCode, int? reportId)
    {
      // su dung datasource config de lay du lieu
      string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      // store get du lieu
      string sqlStore = "NET_Display_WithReport_sel";
      // khai bao param lien quan
      var parameters = new Dictionary<string, object>();
      parameters.Add("ReportCode", reportCode);
      parameters.Add("ReportId", reportId);

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return resultList;
    }
  }
}
