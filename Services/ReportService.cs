
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
using System.ComponentModel;
using LicenseContext = OfficeOpenXml.LicenseContext;

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
    /////////////////////////////////////// cac chuc nang xu ly cau hinh report ////////////////////////////////////
    public Task<int?> GetReportIdFromCode(string reportCode);
    public Task<IDictionary<string, object>?> NET_Report_Get(string reportCode);
    public Task<List<dynamic>> NET_Filter_WithReport_Get(string? reportCode, int? reportId);
    public Task<List<dynamic>> NET_Display_WithReport_Get(Dictionary<string, object>? parameters, string? reportCode, int? reportId);
    // lay danh sach bo loc mac dinh
    public Task<IDictionary<string, object>?> NET_Report_GetDefaultFilter(Dictionary<string, object>? parameters, string sqlStore, string? connectionString);
    // Hàm này sẽ tính độ sâu tối đa của cây phân cấp
    // dung de tính số cấp cha con của cột hiển thị
    public int Display_GetReportMaxParentLevel(List<dynamic> displayList);
    public Task<IDictionary<string, object>?> NET_Report_GetValidation(string reportCode);
  }
  public class ReportService : IReportService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly TttConfigContext _dbconfig;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    public ReportService(QLKCL_NEWContext db, TttConfigContext dbconfig, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con)
    {
      _db = db;
      _dbconfig = dbconfig;
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
      var result = new Dictionary<int, Dictionary<string, object>>();

      string sqlstore = "";

      // Đọc file Excel
      using (var stream = new MemoryStream())
      {
        await file.CopyToAsync(stream);
        using (var package = new ExcelPackage(stream))
        {
          // Sheet "Import" để lấy tên Store Procedure
          var importSheet = package.Workbook.Worksheets["Import"];
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

            var rowData = new Dictionary<string, object>();

            // Lưu trữ các giá trị vào Dictionary theo tên cột
            for (int col = 1; col <= colCount; col++)
            {
              var key = columnNames[col - 1]; var rawValue = worksheet.Cells[row, col].Value?.ToString().Trim() ?? string.Empty;
              try
              {
                // Nếu rawValue là một JSON object hoặc array hợp lệ, parse thành JToken
                var token = JToken.Parse(rawValue);
                rowData[key] = token;
              }
              catch
              {
                // Nếu không parse được, giữ nguyên như string
                rowData[key] = rawValue;
              }
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

      if (file == null || file.Length == 0 || !file.FileName.EndsWith(".xlsx"))
        throw new InvalidOperationException("File không hợp lệ hoặc không đúng định dạng .xlsx.");

      try
      {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var stream = new MemoryStream())
        {
          await file.CopyToAsync(stream);
          stream.Position = 0;

          using (var package = new ExcelPackage(stream))
          {
            var importSheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Import");
            if (importSheet != null)
            {
              sqlstore = importSheet.Cells[1, 1].Text?.Trim() ?? "";
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException($"Lỗi xử lý file: {ex.Message}", ex);
      }

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
          { "id", Id ?? (object)DBNull.Value },
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

    public async Task<int?> GetReportIdFromCode(string reportCode)
    {
      int? reportId = null;
      reportId = await _dbconfig.NetReports.Where(p => p.Code == reportCode).Select(p => (int?)p.Id).FirstOrDefaultAsync();

      return reportId;
    }
    public async Task<IDictionary<string, object>?> NET_Report_Get(string reportCode)
    {
      // su dung datasource config de lay du lieu
      string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      // store get du lieu
      string sqlStore = "NET_Report_sel";
      // khai bao param lien quan
      var parameters = new Dictionary<string, object>();
      parameters.Add("reportcode", reportCode);

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
      parameters.Add("reportcode", reportCode);
      parameters.Add("reportid", reportId);

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return resultList;
    }
        public async Task<List<dynamic>> NET_Display_WithReport_Get(Dictionary<string, object>? parameters, string? reportCode, int? reportId)
        {
            // su dung datasource config de lay du lieu
            string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
                                                                                              // store get du lieu
            string sqlStore = "Store_DRDisplay";
            // neu co report code thi chuyen ve report id
            if (reportCode != null)
            {
                reportId = await GetReportIdFromCode(reportCode);
            }
            // neu parameter rong thi tu tao 1 parameter moi truyen vao
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }

            // chuyen tat ca param dang co thanh 1 chuoi json va truyen vao bien Param
            var displayParameter = new Dictionary<string, object>();
            if (!parameters.ContainsKey("param"))
            {
                // cac du lieu parameter se add vao store display duoi dang bien param cach nhau bang dau ;
                string displayParamString = string.Join(";", parameters.Select(kvp => $"{kvp.Key}={kvp.Value ?? ""}"));
                parameters.Add("param", displayParamString);
            }
            parameters.Add("reportid", reportId);

            // chuyen thanh cau query tu store va param truyen vao
            var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

            var resultList = new List<dynamic>();

            // xu ly lay du lieu dua truyen store va param truyen vao
            resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

            return resultList;
        }

    public async Task<IDictionary<string, object>?> NET_Report_GetDefaultFilter(Dictionary<string, object>? parameters, string sqlStore, string? connectionString)
    {
      // neu khong truyen connect string thi se lay connection string mac dinh
      if (connectionString == null)
      {
        connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      }

      // neu parameter rong thi tu tao 1 parameter moi truyen vao
      if (parameters == null)
      {
        parameters = new Dictionary<string, object>();
      }

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      // xu ly lay du lieu dua truyen store va param truyen vao
      var result = await _con.Connection_GetSingleDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return result;
    }

    // Hàm này sẽ tính độ sâu tối đa của cây phân cấp
    // dung de tính số cấp của cột hiển thị
    public int Display_GetReportMaxParentLevel(List<dynamic> displayList)
    {
      // B1: Tạo lookup table cho tra cứu nhanh theo Code
      var lookup = displayList.ToDictionary(x => (string)x.code?.ToLower(), x => x);

      // B2: Hàm tính độ sâu của từng item (truy ngược lên parent)
      Dictionary<string, int> depthCache = new(); // Để tránh tính lại

      int GetDepth(string code)
      {
        // in thường trước khi xử lý
        code = code?.ToLower();
        if (depthCache.ContainsKey(code))
          return depthCache[code];

        if (!lookup.ContainsKey(code))
          return 1; // Nếu không tìm thấy code -> root

        var item = lookup[code];
        // in thường trước khi xử lý
        string parentCode = item.parentcode?.ToLower() as string;

        int depth = 1;
        if (!string.IsNullOrEmpty(parentCode) && lookup.ContainsKey(parentCode))
        {
          depth = 1 + GetDepth(parentCode);
        }

        depthCache[code] = depth;
        return depth;
      }

      // B3: Duyệt tất cả items và lấy độ sâu lớn nhất
      int maxDepth = 0;
      foreach (var item in displayList)
      {
        string code = item.code?.ToLower() as string;
        if (!string.IsNullOrEmpty(code))
        {
          int depth = GetDepth(code);
          if (depth > maxDepth)
            maxDepth = depth;
        }
      }

      return maxDepth;
    }

    public async Task<IDictionary<string, object>?> NET_Report_GetValidation(string reportCode)
    {
      // su dung datasource config de lay du lieu
      string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      // store get du lieu
      string sqlStore = "net_report_getvalidation";
      // khai bao param lien quan
      var parameters = new Dictionary<string, object>();
      parameters.Add("reportcode", reportCode);

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      // xu ly lay du lieu dua truyen store va param truyen vao
      var result = await _con.Connection_GetSingleDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      return result;
    }
  }
}
