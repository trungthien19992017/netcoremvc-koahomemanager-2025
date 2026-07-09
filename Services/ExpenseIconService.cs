using Newtonsoft.Json;

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

    // Whitelist icon để validate kết quả AI trả về (tránh AI "sáng tạo" class không tồn tại)
    private static readonly HashSet<string> _allowedIcons = new()
        {
            "fa-utensils", "fa-car", "fa-home", "fa-shopping-cart", "fa-plane",
            "fa-heartbeat", "fa-graduation-cap", "fa-film", "fa-mobile-alt",
            "fa-gift", "fa-briefcase", "fa-bolt", "fa-tint", "fa-wifi",
            "fa-coffee", "fa-money-bill-wave", "fa-tshirt", "fa-tools",
            "fa-baby", "fa-paw", "fa-glass-cheers", "fa-book"
        };

    private const string SystemPrompt = """
            Bạn là bộ phân loại chi phí cho phần mềm quản lý chi tiêu.
            Với tên chi phí do người dùng nhập, hãy trả về DUY NHẤT một JSON object,
            KHÔNG kèm markdown, KHÔNG kèm text giải thích, đúng format:
            {"category": string, "faIcon": string, "colorHex": string}

            Quy định:
            - faIcon CHỈ được chọn 1 trong danh sách sau (không tự tạo thêm):
              fa-utensils, fa-car, fa-home, fa-shopping-cart, fa-plane, fa-heartbeat,
              fa-graduation-cap, fa-film, fa-mobile-alt, fa-gift, fa-briefcase, fa-bolt,
              fa-tint, fa-wifi, fa-coffee, fa-money-bill-wave, fa-tshirt, fa-tools,
              fa-baby, fa-paw, fa-glass-cheers, fa-book
            - colorHex là mã màu hex phù hợp tâm lý màu theo nhóm chi phí
            - category là tên nhóm chi phí ngắn gọn bằng tiếng Việt
            """;

    public ExpenseIconService(Func<string, IAiService> aiServiceFactory, ILogger<ExpenseIconService> logger)
    {
      _aiServiceFactory = aiServiceFactory;
      _logger = logger;
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

        // Validate icon — nếu AI trả icon không có trong whitelist thì fallback
        if (!_allowedIcons.Contains(result.FaIcon))
        {
          _logger.LogWarning($"AI trả icon không hợp lệ: {result.FaIcon} cho '{expenseName}'");
          result.FaIcon = fallback.FaIcon;
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
