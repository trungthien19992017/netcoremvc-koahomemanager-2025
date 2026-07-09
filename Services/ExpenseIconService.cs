using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace KOAHome.Services
{
  public class ExpenseIconResult
  {
    public string Category { get; set; } = "";
    public string FaIcon { get; set; } = "fa-money-bill-wave";
    public string ColorHex { get; set; } = "#6b7280";
  }
  public class Expense
  {
    public int Id { get; set; }
    public string ExpenseName { get; set; } = "";
    public decimal Amount { get; set; }
    public string Category { get; set; } = "Khác";
    public string FaIcon { get; set; } = "fa-money-bill-wave"; // icon mặc định trong lúc chờ AI
    public string ColorHex { get; set; } = "#9ca3af"; // màu xám = "đang chờ phân loại"
    public string Status { get; set; } = "Pending"; // Pending | Done | Failed
    public DateTime CreatedAt { get; set; } = DateTime.Now;
  }

  public interface IExpenseIconService
  {
    Task<ExpenseIconResult> ClassifyAsync(string expenseName, string provider, string model);
  }

  public class ExpenseIconService : IExpenseIconService
  {
    private readonly Func<string, IAiService> _aiServiceFactory;
    private readonly ILogger<ExpenseIconService> _logger;
    private readonly FontAwesomeService _validator;

    private const string SystemPrompt = """
            Bạn là bộ phân loại chi phí cho phần mềm quản lý chi tiêu.
            Với tên chi phí do người dùng nhập, hãy trả về DUY NHẤT một JSON object,
            KHÔNG kèm markdown, KHÔNG kèm text giải thích, đúng format:
            {"category": string, "faIcon": string, "colorHex": string}

            Quy định:
            - faIcon phải là icon có thật của FontAwesome Free 6.
            - Không tự tạo icon.
            - Ưu tiên icon trực quan nhất.

              Ví dụ:

              fa-bolt
              fa-lightbulb
              fa-house
              fa-bed
              fa-gas-pump
              fa-car
              fa-faucet
              fa-utensils
              fa-shirt
              fa-book
              fa-laptop
              fa-server
              fa-wifi
              fa-coins
              ...
            - colorHex là mã màu hex phù hợp tâm lý màu theo nhóm chi phí
            - category là tên nhóm chi phí ngắn gọn bằng tiếng Việt
            """;

    public ExpenseIconService(Func<string, IAiService> aiServiceFactory, ILogger<ExpenseIconService> logger, FontAwesomeService validator)
    {
      _aiServiceFactory = aiServiceFactory;
      _logger = logger;
      _validator = validator;
    }

    public async Task<ExpenseIconResult> ClassifyAsync(string expenseName, string provider, string model)
    {
      var fallback = new ExpenseIconResult
      {
        Category = "Khác",
        FaIcon = "fa-money-bill-wave",
        ColorHex = "#6b7280"
      };

      if (string.IsNullOrWhiteSpace(expenseName)) return fallback;

      try
      {
        var aiService = _aiServiceFactory(provider);
        var raw = await aiService.AskOneShotAsync(SystemPrompt, expenseName, model);

        // AI đôi khi vẫn kèm ```json ... ``` dù đã yêu cầu không, nên strip cho chắc
        var clean = raw.Trim();
        if (clean.StartsWith("```"))
        {
          clean = clean.Replace("```json", "").Replace("```", "").Trim();
        }

        var result = JsonConvert.DeserializeObject<ExpenseIconResult>(clean);

        if (result == null) return fallback;

        if (!_validator.Icons.Contains(result.FaIcon))
        {
          result.FaIcon = "fa-money-bill-wave";
        }

        // Validate color hex cơ bản
        if (string.IsNullOrWhiteSpace(result.ColorHex) || !result.ColorHex.StartsWith("#"))
        {
          result.ColorHex = fallback.ColorHex;
        }

        return result;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Lỗi phân loại icon cho chi phí '{expenseName}'");
        return fallback;
      }
    }
  }
}
