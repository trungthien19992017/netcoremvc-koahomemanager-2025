//using KOAHome.EntityFramework;
//using KOAHome.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Channels;

//public class ExpenseController : Controller
//{
//  private readonly QLKCL_NEWContext _db;
//  private readonly IBackgroundTaskQueue _taskQueue;
//  private readonly IServiceScopeFactory _scopeFactory;

//  public ExpenseController(QLKCL_NEWContext db, IBackgroundTaskQueue taskQueue, IServiceScopeFactory scopeFactory)
//  {
//    _db = db;
//    _taskQueue = taskQueue;
//    _scopeFactory = scopeFactory;
//  }

//  [HttpGet]
//  [AllowAnonymous]
//  public IActionResult Index()
//  {
//    return View();
//  }

//  [HttpPost]
//  public async Task<IActionResult> Create([FromBody] CreateExpenseRequest req)
//  {
//    // 1. Lưu ngay với icon mặc định — KHÔNG đợi AI
//    var expense = new Expense
//    {
//      ExpenseName = req.ExpenseName,
//      Amount = req.Amount,
//      Category = "Đang phân loại...",
//      FaIcon = "fa-spinner",       // icon xoay loading tạm thời
//      ColorHex = "#9ca3af",
//      Status = "Pending"
//    };

//    //_db.Expenses.Add(expense);
//    //await _db.SaveChangesAsync();

//    int expenseId = expense.Id;

//    // 2. Đẩy job phân loại vào queue — chạy ngầm, không block response
//    await _taskQueue.QueueAsync(async ct =>
//    {
//      // Vì DbContext là Scoped, phải tạo scope mới trong background task
//      using var scope = _scopeFactory.CreateScope();
//      var scopedDb = scope.ServiceProvider.GetRequiredService<QLKCL_NEWContext>();
//      var iconService = scope.ServiceProvider.GetRequiredService<IExpenseIconService>();

//      try
//      {
//        var result = await iconService.ClassifyAsync(req.ExpenseName, "gemini", "gemini-2.0-flash");

//        //var e = await scopedDb.Expenses.FindAsync(expenseId);
//        //if (e != null)
//        //{
//        //  e.Category = result.Category;
//        //  e.FaIcon = result.FaIcon;
//        //  e.ColorHex = result.ColorHex;
//        //  e.Status = "Done";
//        //  await scopedDb.SaveChangesAsync();
//        //}
//      }
//      catch
//      {
//        //var e = await scopedDb.Expenses.FindAsync(expenseId);
//        //if (e != null)
//        //{
//        //  e.Status = "Failed";
//        //  e.FaIcon = "fa-money-bill-wave";
//        //  e.ColorHex = "#6b7280";
//        //  e.Category = "Khác";
//        //  await scopedDb.SaveChangesAsync();
//        //}
//      }
//    });

//    // 3. Trả về NGAY, không đợi AI xử lý xong
//    return Json(new
//    {
//      id = expense.Id,
//      expenseName = expense.ExpenseName,
//      amount = expense.Amount,
//      faIcon = expense.FaIcon,
//      colorHex = expense.ColorHex,
//      status = expense.Status
//    });
//  }

//  // Endpoint để frontend polling hỏi kết quả
//  [HttpGet]
//  public async Task<IActionResult> GetStatus(int[] ids)
//  {
//    var results = new
//    {
//      id = 0,
//      status = "Done",
//      faIcon = "fa-money-bill-wave",
//      colorHex = "#6b7280",
//      category = "Khác"
//    };
//    //var results = await _db.Expenses
//    //    .Where(e => ids.Contains(e.Id))
//    //    .Select(e => new
//    //    {
//    //      id = e.Id,
//    //      status = e.Status,
//    //      faIcon = e.FaIcon,
//    //      colorHex = e.ColorHex,
//    //      category = e.Category
//    //    })
//    //    .ToListAsync();

//    return Json(results);
//  }

//  public class CreateExpenseRequest
//  {
//    public string ExpenseName { get; set; } = "";
//    public decimal Amount { get; set; }
//  }
//}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

public class ExpenseController : Controller
{
  // Giả lập "DB" tạm trong RAM — dữ liệu sẽ mất khi restart app
  // Dùng static để giữ dữ liệu qua các request khác nhau
  private static readonly ConcurrentDictionary<int, MockExpense> _mockDb = new();
  private static int _nextId = 1;

  public class MockExpense
  {
    public int Id { get; set; }
    public string ExpenseName { get; set; } = "";
    public decimal Amount { get; set; }
    public string Category { get; set; } = "Đang phân loại...";
    public string FaIcon { get; set; } = "fa-spinner";
    public string ColorHex { get; set; } = "#9ca3af";
    public string Status { get; set; } = "Pending";
  }

  [HttpGet]
  [AllowAnonymous]
  public IActionResult Index()
  {
    return View();
  }

  [HttpPost]
  public IActionResult Create([FromBody] CreateExpenseRequest req)
  {
    int id = Interlocked.Increment(ref _nextId);

    var expense = new MockExpense
    {
      Id = id,
      ExpenseName = req.ExpenseName,
      Amount = req.Amount,
      Category = "Đang phân loại...",
      FaIcon = "fa-spinner",
      ColorHex = "#9ca3af",
      Status = "Pending"
    };

    _mockDb[id] = expense;

    // Giả lập AI xử lý ngầm sau 3 giây — KHÔNG dùng "_ =" (fire-and-forget nguy hiểm),
    // dùng Task.Run tách khỏi request context để mô phỏng background job
    _ = SimulateAiClassifyAsync(id, req.ExpenseName);

    return Json(new
    {
      id = expense.Id,
      expenseName = expense.ExpenseName,
      amount = expense.Amount,
      faIcon = expense.FaIcon,
      colorHex = expense.ColorHex,
      status = expense.Status,
      category = expense.Category
    });
  }

  [HttpGet]
  public IActionResult GetStatus(int[] ids)
  {
    var results = ids
        .Where(id => _mockDb.ContainsKey(id))
        .Select(id => _mockDb[id])
        .Select(e => new
        {
          id = e.Id,
          status = e.Status,
          faIcon = e.FaIcon,
          colorHex = e.ColorHex,
          category = e.Category
        })
        .ToList();

    return Json(results);
  }

  // Hàm giả lập việc gọi AI mất vài giây rồi trả kết quả
  private async Task SimulateAiClassifyAsync(int id, string expenseName)
  {
    await Task.Delay(3000); // giả lập độ trễ gọi AI thật (3 giây)

    // Giả lập map cứng vài từ khóa để test trực quan, không cần gọi AI thật
    var (icon, color, category) = MockClassify(expenseName);

    if (_mockDb.TryGetValue(id, out var expense))
    {
      expense.FaIcon = icon;
      expense.ColorHex = color;
      expense.Category = category;
      expense.Status = "Done";
    }
  }

  private (string icon, string color, string category) MockClassify(string name)
  {
    var lower = name.ToLower();

    if (lower.Contains("cà phê") || lower.Contains("ăn") || lower.Contains("trà"))
      return ("fa-utensils", "#f59e0b", "Ăn uống");

    if (lower.Contains("xăng") || lower.Contains("grab") || lower.Contains("taxi"))
      return ("fa-car", "#3b82f6", "Di chuyển");

    if (lower.Contains("điện") || lower.Contains("nước") || lower.Contains("wifi"))
      return ("fa-bolt", "#eab308", "Hóa đơn");

    return ("fa-money-bill-wave", "#6b7280", "Khác");
  }

  public class CreateExpenseRequest
  {
    public string ExpenseName { get; set; } = "";
    public decimal Amount { get; set; }
  }
}
