
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
using KOAHome.Helpers;
using Npgsql;
using System.Text.Json;
using System.Web;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
    public Task<string> BuildHtmlTableRows(
        List<dynamic> resultList,
        List<dynamic> displayList,
        List<dynamic> actionlistdetailList,
        Dictionary<string, object> listFilterValue,
        Dictionary<string, List<SelectListItem>> editorDynamicServiceSelectOptions);
    public Task<string> BuildRowAsync
       (
        dynamic result,
        List<dynamic> displayList,
        List<dynamic> actionlistdetailList,
        Dictionary<string, object> listFilterValue,
        Dictionary<string, List<SelectListItem>> editorDynamicServiceSelectOptions,
        int index
    );
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
            var importSheet = package.Workbook.Worksheets["Import"];
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
    public async Task<string> BuildHtmlTableRows
      (
        List<dynamic> resultList,
        List<dynamic> displayList,
        List<dynamic> actionlistdetailList,
        Dictionary<string, object> listFilterValue,
        Dictionary<string, List<SelectListItem>> editorDynamicServiceSelectOptions
      )
    {
      var sb = new StringBuilder();
      var tasks = new List<Task<string>>(); // Ensure the type is Task<string>  

      var index = 0; // Chỉ số dòng gốc  

      // Lặp qua từng dòng trong resultList và tạo các task song song  
      foreach (var result in resultList)
      {
        var currentIndex = index++; // Lưu giữ chỉ số dòng gốc  
        tasks.Add(BuildRowAsync(result, displayList, actionlistdetailList, listFilterValue, editorDynamicServiceSelectOptions, currentIndex));
      }

      // Chờ tất cả các task hoàn thành và kết hợp kết quả  
      var results = await Task.WhenAll(tasks);

      // Kết hợp tất cả các kết quả lại trong đúng thứ tự  
      foreach (var result in results)
      {
        sb.Append(result);
      }

      return sb.ToString();
    }


    public async Task<string> BuildRowAsync
    (
        dynamic result,
        List<dynamic> displayList,
        List<dynamic> actionlistdetailList,
        Dictionary<string, object> listFilterValue,
        Dictionary<string, List<SelectListItem>> editorDynamicServiceSelectOptions,
        int index
    )
    {
      var sb = new StringBuilder();

      // Tiến hành xử lý dòng, tương tự như trong logic trước đó
      var rowDict = (IDictionary<string, object>)result;
      var rowId = rowDict.ContainsKey("id") ? Convert.ToInt32(rowDict["id"]) : 0;
      var bgColorRowClass = rowDict.ContainsKey("bgcolorrowclass") ? rowDict["bgcolorrowclass"]?.ToString() : "";

      sb.AppendLine($"<tr class='{bgColorRowClass}'>");

      // Cột select box mặc định
      sb.AppendLine($"<td class='text-center' data-isexport='False' data-value='{rowId}'></td>");

      // Cột chi tiết mặc định
      sb.AppendLine("<td class='text-center' data-isexport='False'></td>");

      // Cột dữ liệu tĩnh (cột display == false)
      var staticColumns = displayList
          .Where(p => p.isdisplay == false)
          .OrderBy(p => p.colnum ?? 0);

      foreach (var display in staticColumns)
      {
        string fieldCode = display.code?.ToLower() ?? "";
        string value = rowDict.TryGetValue(fieldCode, out var rawVal)
            ? rawVal?.ToString() ?? "N/A"
            : "N/A";

        string tdClass = "d-none " + FormatHelper.ParseTextAlignToClass(display.textalign);
        string isExport = display.isexport?.ToString() ?? "True";

        sb.AppendLine($"<td class='{tdClass}' data-isexport='{isExport}'>{value}</td>");
      }

      // Các cột STT nếu có
      var sttCols = displayList
          .Where(p => p.isdisplay == true && (p.code?.ToLower() == "stt") && p.isparent == false && string.IsNullOrEmpty(p.parentcode))
          .OrderBy(p => p.colnum ?? 0);

      foreach (var display in sttCols)
      {
        var css = display.isfreepane == true ? "freepanze-col" : "";
        var isExport = display.isexport?.ToString() ?? "True";

        string fieldCode = display.code?.ToLower() ?? "";
        string value = rowDict.TryGetValue(fieldCode, out var rawVal)
            ? rawVal?.ToString() ?? "N/A"
            : "N/A";

        sb.AppendLine($"<td class='text-center {css}' data-isexport='{isExport}'>{HttpUtility.HtmlEncode(value)}</td>");
      }

      // Xử lý dropdown action list từ actionListDetailGrid
      sb.AppendLine("<td class='text-center' data-isexport='False'>");
      sb.AppendLine("<div class='demo-inline-spacing'>");
      sb.AppendLine("<div class='btn-group' id='dropdown-icon-demo'>");
      sb.AppendLine("<button type='button' class='btn btn-primary btn-xs dropdown-toggle' data-bs-toggle='dropdown' aria-expanded='false'><i class='ri-align-justify me-1'></i></button>");
      sb.AppendLine("<ul class='dropdown-menu overflow-auto' style='max-height:200px'>");

      var actionlistdetailList_grid = null as List<dynamic>;
      // neu ton tai action list detail
      if (actionlistdetailList != null)
      {
        // phan biet action top va grid
        actionlistdetailList_grid = actionlistdetailList.Where(p => p.istop == null || p.istop == false).ToList();
      }

      foreach (var action in actionlistdetailList_grid.OrderBy(p => p.actionlistdetailorderid))
      {
        var actionDict = (IDictionary<string, object>)action;
        string actionName = actionDict.ContainsKey("actionname") ? (actionDict["actionname"].ToString() ?? "") : "";
        string actionCode = actionDict.ContainsKey("actioncode") ? (actionDict["actioncode"].ToString() ?? "") : "";
        bool actionIsTop = actionDict.ContainsKey("istop") ? Convert.ToBoolean(actionDict["istop"]) : false;
        string actionIcon = actionDict.ContainsKey("actionicon") ? (actionDict["actionicon"].ToString() ?? "") : "";
        string actionType = actionDict["type"].ToString() ?? "";
        string actionValue = actionDict["value"].ToString() ?? "";

        if (actionType == "LINK")
        {
          string url = ParseDataHelper.GetReplaceLinkWithResult(actionValue, rowDict, listFilterValue);
          sb.AppendLine($"<li><a href='{url}' class='dropdown-item'><i class='{actionIcon + " me-1 text-primary"}'></i> {actionName}</a></li>");
        }
        else if (actionType == "STORE")
        {
          string actionStore = actionValue;
          bool isPopupConfirm = Convert.ToBoolean(actionDict["ispopupconfirm"]);
          int? dataSourceID = actionDict.TryGetValue("datasourceid", out var num) && num is int val ? Convert.ToInt32(num) : null;
          string confirmButtonText = !string.IsNullOrWhiteSpace(actionDict["confirmbuttontext"].ToString()) ? actionDict["confirmbuttontext"].ToString() : null;
          string confirmTitle = !string.IsNullOrWhiteSpace(actionDict["confirmtitle"].ToString()) ? actionDict["confirmtitle"].ToString() : null;
          string confirmText = !string.IsNullOrWhiteSpace(actionDict["confirmtext"].ToString()) ? actionDict["confirmtext"].ToString() : null;
          sb.AppendLine("<li>");
          sb.AppendLine(isPopupConfirm
              ? $"<a href='' data-id='{rowId}' data-isconfirm='{isPopupConfirm}' data-confirmtext='{confirmText}' data-confirmtitle='{confirmTitle}' data-confirmbutton='{confirmButtonText}' data-sqlstore='{actionStore}' data-datasourceid='{dataSourceID}' class='dropdown-item confirmAction'><i class='{actionIcon + " me-1 text-primary"}'></i> {actionName}</a>"
              : $"<a href='' data-id='{rowId}' data-sqlstore='{actionStore}' data-datasourceid='{dataSourceID}' class='dropdown-item confirmAction'><i class='{actionIcon + " me-1 text-primary"}'></i> {actionName}</a>");
          sb.AppendLine("</li>");
        }
        else if (actionType == "POPUPFORM")
        {
          var popupDictValue = ParseDataHelper.GetReplacePopupLinkWithResult(actionValue, rowDict, listFilterValue);
          string actionFormCode = popupDictValue.ContainsKey("FORMCODE") ? popupDictValue["FORMCODE"].ToString() ?? "" : "";
          string actionLink = popupDictValue.ContainsKey("LINK") ? popupDictValue["LINK"].ToString() ?? "" : "";
          sb.AppendLine($"<li><a href='' data-bs-toggle='modal' data-bs-target='#modal-PopupForm' data-actionlink='{actionLink}' class='dropdown-item popupform-action'><i class='{actionIcon + " me-1 text-primary"}'></i> {actionName}</a></li>");
        }
        else
        {
          string url = ParseDataHelper.GetReplaceLinkWithResult(actionValue, rowDict, listFilterValue);
          sb.AppendLine($"<li><a href='{url}' class='dropdown-item'><i class='{actionIcon + " me-1 text-primary"}'></i> {actionName}</a></li>");
        }
      }

      sb.AppendLine("</ul>");
      sb.AppendLine("</div>");
      sb.AppendLine("</div>");
      sb.AppendLine("</td>");

      // Xử lý cột không có parentcode và không phải là parent
      var columnsWithoutParent = displayList
          .Where(p => p.isdisplay == true && p.code?.ToLower() != "stt" && p.isparent == false && string.IsNullOrEmpty(p.parentcode))
          .OrderBy(p => p.colnum ?? 0);

      foreach (var display in columnsWithoutParent)
      {
        var displayType = display.type as string ?? "link";
        var displayFormat = display.format as string ?? "";
        var displayCode = display.code.ToLower() as string ?? "";
        var displayTextAlign = display.textalign as string ?? "";
        var displayIsExport = display.isexport;
        var displayIsReadOnly = display.isreadonly ?? false;
        var displayValidationRule = display.validationrule ?? "[]";

        // Kiểm tra nếu là FreePane
        var freePaneClass = display.isfreepane == true ? "freepanze-col" : "";
        sb.AppendLine($"<td class='{FormatHelper.ParseTextAlignToClass(displayTextAlign)} {freePaneClass}' data-isexport='{displayIsExport ?? "True"}' data-displaytype='{displayType}' data-displayformat='{displayFormat}'>");

        object value;
        if (rowDict.TryGetValue(displayCode, out value))
        {
          string valueType = value?.GetType().ToString() ?? "";
          // kiem tra kieu du lieu cua display
          string[] stringVavlidType = ["string", "date", "datetime", "int", "long", "float", "textarea"];
          // Kiểm tra kiểu dữ liệu của value và xử lý
          if (valueType == "System.Boolean")
          {
            sb.AppendLine($"<input type='checkbox' {(Convert.ToBoolean(value) ? "checked" : "")} disabled />");
          }
          else if (displayType == "icons")
          {
            var iconsJson = value?.ToString()?.Trim() ?? "";
            if (!string.IsNullOrEmpty(iconsJson))
            {
              if (iconsJson.TrimStart().StartsWith("{"))
              {
                iconsJson = "[" + iconsJson + "]";
              }

              try
              {
                var icons = JsonSerializer.Deserialize<List<JsonElement>>(iconsJson);
                sb.AppendLine("<ul class='list-unstyled m-0 avatar-group d-flex align-items-center'>");
                foreach (var icon in icons)
                {
                  var title = icon.GetProperty("title").GetString();
                  var image = icon.GetProperty("image").GetString();
                  sb.AppendLine($"<li data-bs-toggle='tooltip' class='avatar avatar-xs pull-up' aria-label='{title}'><img src='{image}' alt='Avatar' class='rounded-circle' /></li>");
                }
                sb.AppendLine("</ul>");
              }
              catch (PostgresException)
              {
                sb.AppendLine("<span class='text-danger'>N/A</span>");
              }
            }
          }
          else if (displayType == "file")
          {
            var filesJson = value?.ToString()?.Trim() ?? "";
            if (!string.IsNullOrEmpty(filesJson))
            {
              if (filesJson.StartsWith("{"))
              {
                filesJson = "[" + filesJson + "]";
              }

              try
              {
                var fileList = JsonSerializer.Deserialize<List<JsonElement>>(filesJson);
                if (fileList?.Count > 0)
                {
                  sb.AppendLine("<div class='d-flex flex-wrap gap-1'>");
                  foreach (var file in fileList)
                  {
                    var fileName = file.GetProperty("fileName").GetString();
                    var url = file.GetProperty("url").GetString();
                    var displayName = fileName?.Length > 50 ? fileName.Substring(0, 50) + "..." : fileName;
                    sb.AppendLine($"<a href='{url}' download class='btn btn-sm btn-outline-primary file-download-link' title='{fileName}'><i class='ri-download-2-fill'></i> {displayName}</a>");
                  }
                  sb.AppendLine("</div>");
                }
              }
              catch
              {
                sb.AppendLine("<span class='text-danger'>Lỗi định dạng file JSON</span>");
              }
            }
          }
          else if (displayType == "link")
          {
            sb.AppendLine($"{HttpUtility.HtmlEncode(value)}");
          }
          // neu la kieu du lieu co the format duoc
          else if (stringVavlidType.Contains(displayType))
          {
            // kiem tra neu co format thi format lai chuoi
            var displayValue = @FormatHelper.FormatDynamicValue(value, displayFormat);
            sb.AppendLine($"<span style='white-space: pre-wrap'>{displayValue}</span>");

          }
          else if (displayType == "combobox")
          {
            // check selected cho item dung voi gia tri
            var selectList = editorDynamicServiceSelectOptions[displayCode] as List<SelectListItem> ?? new List<SelectListItem>();
            // lay du lieu text từ service thông qua value
            string displayValue = selectList.Where(p => p.Value.ToString() == value.ToString()).Select(p => p.Text).First().ToString() ?? "";
            sb.AppendLine($"<span style='white-space: pre-wrap'>{displayValue}</span>");
          }
          else
          {
            sb.AppendLine(HttpUtility.HtmlEncode(value));
          }
        }
        else
        {
          sb.AppendLine("<span class='text-muted'>N/A</span>");
        }

        sb.AppendLine("</td>");
      }
      // **Xử lý cột có isparent == true và các cột con (2 cấp)**
      var parentColumns = displayList
          .Where(p => p.isparent == true && p.isdisplay == true)
          .OrderBy(p => p.colnum ?? 0);

      foreach (var display in parentColumns)
      {
        // Đếm số cột con (child)
        var countChild = displayList.Count(p => p.parentcode?.ToLower() == display.code.ToLower());

        if (countChild > 0)
        {
          // Hiển thị cột con (child columns)
          var childColumns = displayList
              .Where(p => p.parentcode?.ToLower() == display.code.ToLower() && p.isdisplay == true)
              .OrderBy(p => p.colnum ?? 0);

          foreach (var displaychild in childColumns)
          {

            // mặc định type là string
            var displayType = displaychild.type as string ?? "string";
            var displayFormat = displaychild.format as string ?? "";
            var displayCode = displaychild.code.ToLower() as string ?? "";
            var displayTextAlign = displaychild.textalign as string ?? "";
            var displayIsExport = displaychild.isexport;
            var displayIsReadOnly = displaychild.isreadonly ?? false;
            var displayValidationRule = displaychild.validationrule ?? "[]";

            // Kiểm tra nếu là FreePane
            var freePaneClass = displaychild.isfreepane == true ? "freepanze-col" : "";

            sb.AppendLine($"<td class='{FormatHelper.ParseTextAlignToClass(displayTextAlign)} {freePaneClass}' data-isexport='{displayIsExport ?? "True"}' data-displaytype='{displayType}' data-displayformat='{displayFormat}'>");

            object value;
            if (rowDict.TryGetValue(displayCode, out value))
            {
              string valueType = value?.GetType().ToString() ?? "";
              // kiem tra kieu du lieu cua display
              string[] stringVavlidType = ["string", "date", "datetime", "int", "long", "float", "textarea"];

              // Kiểm tra kiểu dữ liệu của value và xử lý
              if (valueType == "System.Boolean")
              {
                sb.AppendLine($"<input type='checkbox' {(Convert.ToBoolean(value) ? "checked" : "")} disabled />");
              }
              else if (displayType == "icons")
              {
                var iconsJson = value?.ToString()?.Trim() ?? "";
                if (!string.IsNullOrEmpty(iconsJson))
                {
                  if (iconsJson.TrimStart().StartsWith("{"))
                  {
                    iconsJson = "[" + iconsJson + "]";
                  }

                  try
                  {
                    var icons = JsonSerializer.Deserialize<List<JsonElement>>(iconsJson);
                    sb.AppendLine("<ul class='list-unstyled m-0 avatar-group d-flex align-items-center'>");
                    foreach (var icon in icons)
                    {
                      var title = icon.GetProperty("title").GetString();
                      var image = icon.GetProperty("image").GetString();
                      sb.AppendLine($"<li data-bs-toggle='tooltip' class='avatar avatar-xs pull-up' aria-label='{title}'><img src='{image}' alt='Avatar' class='rounded-circle' /></li>");
                    }
                    sb.AppendLine("</ul>");
                  }
                  catch (PostgresException)
                  {
                    sb.AppendLine("<span class='text-danger'>N/A</span>");
                  }
                }
              }
              else if (displayType == "file")
              {
                var filesJson = value?.ToString()?.Trim() ?? "";
                if (!string.IsNullOrEmpty(filesJson))
                {
                  if (filesJson.StartsWith("{"))
                  {
                    filesJson = "[" + filesJson + "]";
                  }

                  try
                  {
                    var fileList = JsonSerializer.Deserialize<List<JsonElement>>(filesJson);
                    if (fileList?.Count > 0)
                    {
                      sb.AppendLine("<div class='d-flex flex-wrap gap-1'>");
                      foreach (var file in fileList)
                      {
                        var fileName = file.GetProperty("fileName").GetString();
                        var url = file.GetProperty("url").GetString();
                        var displayName = fileName?.Length > 50 ? fileName.Substring(0, 50) + "..." : fileName;
                        sb.AppendLine($"<a href='{url}' download class='btn btn-sm btn-outline-primary file-download-link' title='{fileName}'><i class='ri-download-2-fill'></i> {displayName}</a>");
                      }
                      sb.AppendLine("</div>");
                    }
                  }
                  catch
                  {
                    sb.AppendLine("<span class='text-danger'>Lỗi định dạng file JSON</span>");
                  }
                }
              }
              else if (displayType == "link")
              {
                sb.AppendLine($"{HttpUtility.HtmlEncode(value)}");
              }
              // neu la kieu du lieu co the format duoc
              else if (stringVavlidType.Contains(displayType))
              {
                // kiem tra neu co format thi format lai chuoi
                var displayValue = @FormatHelper.FormatDynamicValue(value, displayFormat);
                sb.AppendLine($"<span style='white-space: pre-wrap'>{displayValue}</span>");

              }
              else if (displayType == "combobox")
              {
                // check selected cho item dung voi gia tri
                var selectList = editorDynamicServiceSelectOptions[displayCode] as List<SelectListItem> ?? new List<SelectListItem>();
                // lay du lieu text từ service thông qua value
                string displayValue = selectList.Where(p => p.Value.ToString() == value.ToString()).Select(p => p.Text).First().ToString() ?? "";
                sb.AppendLine($"<span style='white-space: pre-wrap'>{displayValue}</span>");
              }
              else
              {
                sb.AppendLine(HttpUtility.HtmlEncode(value));
              }
            }
            else
            {
              sb.AppendLine("<span class='text-muted'>N/A</span>");
            }
            sb.AppendLine("</td>");
          }
        }
      }
      sb.AppendLine("</tr>");

      return sb.ToString();
    }
  }
}
