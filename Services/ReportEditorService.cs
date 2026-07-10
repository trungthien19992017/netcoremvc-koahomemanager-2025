
using Google.Api;
using Google.Apis.Sheets.v4.Data;
using KOAHome.EntityFramework;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;

namespace KOAHome.Services
{
  public interface IReportEditorService
  {
    public Task<string> ExtractGridDataToJson(IFormCollection form);
    public Task<List<dynamic>> ReportEditor_Json_Update(Dictionary<string, object>? parameters, int? Id, string json, string sqlStore, string? connectionString);
    public Task<string> AIResponse(string provider, string model, string systemPrompt, string request);
    public Task<IFormCollection> ProcessFormWithAIAsync(IFormCollection form, string aiRequestColumn, string systemPrompt, string provider, string model);
    public Task<string> GetSystemPrompt(string storeName, string? jsonParam);

  }
  public class ReportEditorService : IReportEditorService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    private readonly Func<string, IAiService> _aiServiceFactory;
    private readonly ILogger<ExpenseIconService> _logger;
    private readonly FontAwesomeService _validator;
    public ReportEditorService(QLKCL_NEWContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con, Func<string, IAiService> aiServiceFactory, ILogger<ExpenseIconService> logger, FontAwesomeService validator)
    {
      _db = db;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
      _con = con;
      _aiServiceFactory = aiServiceFactory;
      _logger = logger;
      _validator = validator;
    }
    public async Task<string> ExtractGridDataToJson(IFormCollection form)
    {
      // Dictionary để nhóm dữ liệu theo số thứ tự [n]
      var gridData = new Dictionary<int, Dictionary<string, string>>();

      foreach (var key in form.Keys)
      {
        var match = Regex.Match(key, @"grid\[(\d+)\]\.(\w+)");
        if (match.Success)
        {
          int index = int.Parse(match.Groups[1].Value); // Lấy số thứ tự n
          string field = match.Groups[2].Value; // Lấy tên cột (Id, Quantity, Description,...)
          string value = form[key].ToString();

          if (!gridData.ContainsKey(index))
          {
            gridData[index] = new Dictionary<string, string>();
          }

          // Nếu giá trị là chuỗi rỗng, gán `null`
          if (string.IsNullOrEmpty(value))
          {
            value = null;
          }
          else 
          // Kiểm tra nếu field chứa "date" thì format thành yyyy-MM-dd HH:mm:ss
          if (field.IndexOf("date", StringComparison.OrdinalIgnoreCase) >= 0)
          {
            if (DateTime.TryParse(value, out DateTime parsedDate))
            {
              value = parsedDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
          }

          gridData[index][field] = value; // Gán giá trị vào dictionary
        }
      }

      // Chuyển đổi dữ liệu sang JSON
      return await Task.FromResult(JsonConvert.SerializeObject(gridData.Values, Formatting.Indented));
    }

    public async Task<List<dynamic>> ReportEditor_Json_Update(Dictionary<string, object>? parameters, int? Id, string json, string sqlStore, string? connectionString)
    {
      if (parameters == null)
      {
        parameters = new Dictionary<string, object>();
      }
      // nếu chưa tồn tại Id thì thêm vào
      if (!parameters.ContainsKey("id"))
      {
        parameters.Add("id", Id ?? (object)DBNull.Value);
      }
      // them chuỗi json xử lý dữ liệu
      parameters.Add("json", string.IsNullOrEmpty(json) ? (object)DBNull.Value : json);

      // neu khong truyen connect string thi se lay connection string mac dinh
      if (connectionString == null)
      {
        connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      }

      //voi du lieu param lay tu store, kiem tra dinh dang de xu ly
      // kiem tra du lieu nhap vao co trong store thi dua vao lenh xu ly
      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);
      return resultList;
    }
    public async Task<string> AIResponse(string provider = "openrouter", string model = "deepseek/deepseek-chat", string systemPrompt = "Hãy trả về DUY NHẤT một JSON object,KHÔNG kèm markdown, KHÔNG kèm text giải thích, đúng format: {}", string request = "")
    {

      var aiService = _aiServiceFactory(provider);
      var raw = await aiService.AskOneShotAsync(systemPrompt, request, model);

      var clean = raw.Trim();
      if (clean.StartsWith("```"))
      {
        clean = clean.Replace("```json", "").Replace("```", "").Trim();
      }
      _logger.LogInformation("Request: " + request + ", AIResponse: " + clean);
      return clean;
    }

    public async Task<IFormCollection> ProcessFormWithAIAsync(IFormCollection form, string aiRequestColumn, string systemPrompt, string provider = "openrouter", string model = "deepseek/deepseek-chat")
    {
      // 1. Tách danh sách các cột cần lấy dữ liệu làm request
      var requestCols = aiRequestColumn.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

      // Regex để bắt cấu trúc dòng: grid[X].y_z_w
      var gridRegex = new Regex(@"^grid\[(?<index>\d+)\]\.(?<column>.+)$");

      // 2. Gom nhóm các key trong Form theo index của từng dòng
      var rowGroups = form.Keys
          .Select(key => gridRegex.Match(key))
          .Where(m => m.Success)
          .GroupBy(
              m => m.Groups["index"].Value,
              m => new { CoreColumn = m.Groups["column"].Value, FullKey = m.Value }
          );

      // Chuẩn bị danh sách các task xử lý AI song song để tối ưu thời gian
      var tasks = rowGroups.Select(async group =>
      {
        var rowIndex = group.Key;

        // Tạo dictionary chứa dữ liệu của các cột được yêu cầu cho dòng hiện tại
        var requestData = group
            .Where(g => requestCols.Contains(g.CoreColumn))
            .ToDictionary(g => g.CoreColumn, g => form[g.FullKey].ToString());

        // Nếu dòng này không chứa bất kỳ cột nào cần thiết, bỏ qua không gọi AI
        if (!requestData.Any()) return null;

        // Convert thành chuỗi JSON request dạng: {"content":"Tiền điện tháng 5", "quantity":"1"}
        string aiRequestJson = JsonConvert.SerializeObject(requestData, Formatting.Indented);

        // Gọi AI xử lý
        var aiService = _aiServiceFactory(provider);
        string aiResult = await aiService.AskOneShotAsync(systemPrompt, aiRequestJson, model);

        // Trả về kết quả kèm theo Key cần map lại vào Form
        return new { TargetKey = $"grid[{rowIndex}].airesponsejson", Value = aiResult };
      });

      // Chạy song song toàn bộ các dòng
      var results = await Task.WhenAll(tasks);

      // 3. Tạo FormCollection mới để return (vì IFormCollection gốc là ReadOnly)
      var newFields = form.ToDictionary(k => k.Key, k => k.Value);

      foreach (var res in results.Where(r => r != null))
      {
        // Gán hoặc cập nhật đè cột airesponsejson
        newFields[res.TargetKey] = res.Value;
      }

      return new FormCollection(newFields, form.Files);
    }

    public async Task<string> GetSystemPrompt(string storeName, string? jsonParam)
    {
      string connectionString = _configuration.GetConnectionString("DefaultConnection"); 
      string sqlStore = storeName;

      var parameters = new Dictionary<string, object>();
      parameters.Add("json", jsonParam ?? "");
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);
      var result = await _con.Connection_GetSingleDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

      var data = result.ContainsKey("promptdata") ? Convert.ToString(result["promptdata"]) ?? "" : "";

      return data;
    }
  }
}
