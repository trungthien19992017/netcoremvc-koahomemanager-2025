using Amazon.Runtime.Internal.Util;
using Google.GenAI;
using KOAHome.EntityFramework;
using KOAHome.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Npgsql;
using OpenAI;
using System.Text;
using System.Text.Json;
using GoogleGenAIType = Google.GenAI.Types;

namespace KOAHome.Services
{
  public interface IAiService
  {
    public Task<string> AskAsync(string message, string prompt, string selectedModel);
    public Task<string> AskOneShotAsync(string systemPrompt, string userMessage, string selectedModel);
    public string BuildGuestPrompt(int bookingID, string userMessage);
    public Task<string> BuildGuestPromptByPhone(string phoneNumber, string userMessage);
    public Task<List<ChatHistoryModel>> GetChatHistory();
  }
  public class GeminiService : IAiService
  {
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private readonly QLKCL_NEWContext _db;
    private readonly IConnectionService _con;
    private readonly ILogger<IAiService> _logger;
    private readonly IDistributedCache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GeminiService(HttpClient http, IConfiguration config, QLKCL_NEWContext db, IConnectionService con, ILogger<IAiService> logger, IDistributedCache cache, IHttpContextAccessor httpContextAccessor)
    {
      _http = http;
      _config = config;
      _db = db;
      _con = con;
      _logger = logger;
      _cache = cache;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> AskAsync(string message, string prompt, string selectedModel)
    {
      string uniqueKey = GetVisitorId();
      var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
      var apiKey = "";
      if (env == "Development")
      {
        apiKey = _config["Gemini:ApiKey"];
      }
      else
      {
        apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
      }

      string cacheKey = $"History_Gemini_{uniqueKey}";

      // 1. Lấy lịch sử từ Redis
      var cachedData = await _cache.GetStringAsync(cacheKey);
      List<ChatHistoryModel> history = cachedData != null
          ? JsonConvert.DeserializeObject<List<ChatHistoryModel>>(cachedData)
          : new List<ChatHistoryModel>();

      // 2. Thêm tin nhắn mới của User vào lịch sử
      history.Add(new ChatHistoryModel { Role = "user", Parts = message });

      // Giới hạn lịch sử (Ví dụ: Chỉ lấy 10 câu gần nhất để tiết kiệm token)
      if (history.Count > 10) history = history.TakeLast(10).ToList();

      var geminiHistory = history.Select(h => new GoogleGenAIType.Content
      {
        Role = h.Role, // "user" hoặc "model"
        Parts = new List<GoogleGenAIType.Part>
        {
            new GoogleGenAIType.Part { Text = h.Parts }
        }
        }).ToList();

      var generateContentConfig = new GoogleGenAIType.GenerateContentConfig
      {
        // Đây chính là Role "System" - định hình tính cách và dữ liệu gốc
        SystemInstruction = new GoogleGenAIType.Content
        {
          Parts = new List<GoogleGenAIType.Part> { new GoogleGenAIType.Part { Text = prompt } }
        }
      };
      // 3. Chuẩn bị gọi Gemini (Gửi kèm System Prompt và History)
      // Lưu ý: Ở đây bạn cần map history này vào object 'ChatHistoryModels' của Gemini API
      var client = new Client(apiKey: apiKey);
      var respone = await client.Models.GenerateContentAsync(
        model: selectedModel,
        contents: geminiHistory,
        config: generateContentConfig
        );
      var botResponse = respone.Candidates[0].Content.Parts[0].Text;

      // Log query một lần khi trợ lý trả lời
      string singleLineCusContent = message.ToString().Replace(Environment.NewLine, " ").Replace("\n", " ");
      string singleLineBotContent = botResponse.ToString().Replace(Environment.NewLine, " ").Replace("\n", " ");
      _logger.LogInformation($"Khách hỏi: '{singleLineCusContent}' - Bot trả lời '{singleLineBotContent}'");

      // 4. Cập nhật câu trả lời của Bot vào lịch sử
      history.Add(new ChatHistoryModel { Role = "model", Parts = botResponse });

      // 5. Lưu lại vào Redis (Set thời gian hết hạn 30 phút - Sliding Expiration)
      var cacheOptions = new DistributedCacheEntryOptions
      {
        SlidingExpiration = TimeSpan.FromMinutes(30)
      };
      await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(history), cacheOptions);

      // 6. Lưu vào DB (Background Task) - Để phân tích nhu cầu khách hàng sau này
      _ = SaveToDatabaseAsync(uniqueKey, message, botResponse);

      return botResponse;
    }

    private async Task SaveToDatabaseAsync(string sessionId, string userMsg, string botMsg)
    {
      // Code lưu vào bảng ChatLogs của bạn
      // Nên dùng một Queue hoặc BackgroundService để không làm chậm trải nghiệm khách
    }

    private string GetVisitorId()
    {
      const string CookieName = "KOA_Visitor_Identity";

      // 1. Thử lấy Key từ Cookie của trình duyệt gửi lên
      var visitorId = _httpContextAccessor.HttpContext.Request.Cookies[CookieName];

      if (string.IsNullOrEmpty(visitorId))
      {
        // 2. Nếu chưa có (khách mới), tạo một mã định danh duy nhất (GUID)
        visitorId = Guid.NewGuid().ToString();

        // 3. Cấu hình Cookie để lưu lại trên máy khách
        var cookieOptions = new CookieOptions
        {
          Path = "/",
          HttpOnly = true, // Bảo mật, không cho script đọc
          IsEssential = true, // Hoạt động ngay cả khi khách chưa đồng ý cookie (tùy luật GDPR)
          Expires = DateTimeOffset.UtcNow.AddMinutes(30) // Lưu trong 30 phút, sau đó sẽ hết hạn nếu khách không quay lại
        };

        // 4. Gửi Cookie về trình duyệt
        _httpContextAccessor.HttpContext.Response.Cookies.Append(CookieName, visitorId, cookieOptions);
      }

      return visitorId;
    }

    public string BuildGuestPrompt(int bookingID, string userMessage)
    {
      var customer = new HsCustomer();
      var room = new HsRoom();
      var booking = _db.HsBookings.FirstOrDefault(p => p.Bookingid == bookingID);
      if (booking != null)
      {
        customer = _db.HsCustomers.FirstOrDefault(p => p.Customerid == booking.Customerid);
        room = _db.HsRooms.FirstOrDefault(p => p.Roomid == booking.Roomid);
      }
      return $"""
        Bạn là trợ lý ảo của homestay KOA Home.

        Thông tin đặt phòng:
        - Tên khách: {customer.Firstname} {customer.Lastname}
        - Phòng: {room.Name}
        - Tầng: {room.Floor}
        - Số điện thoại: {customer.Phonenumber}
        - Check-in: {booking.Checkindate:dd/MM/yyyy HH:mm}
        - Check-out: {booking.Checkoutdate:dd/MM/yyyy HH:mm}
        - Trạng thái thanh toán (Đã thanh toán hết hay chưa?): {booking.Ispay}
        - Tổng số tiền: {booking.Totalamount}
        - Cọc: {booking.Deposit}
        - Số tiền còn lại phải thanh toán: {(booking.Ispay == true ? 0 :booking.Totalamount - booking.Deposit)}
        - Nội quy: Không hút thuốc, không thú cưng, giữ yên tĩnh sau 22h

        Khách hỏi:
        "{userMessage}"

        Yêu cầu trả lời:
        - Ngắn gọn
        - Thân thiện
        - Tiếng Việt
        - Không nhắc đến AI
        """;
    }

    public async Task<string> BuildGuestPromptByPhone(string phoneNumber, string userMessage)
    {
      // 1. Lấy thông tin khách hàng theo số điện thoại
      var customer = _db.HsCustomers
          .FirstOrDefault(c => c.Phonenumber == phoneNumber);

      if (customer == null)
      {
        // neu khong truyen connect string thi se lay connection string mac dinh
        string connectionString = _config.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
        var parameters = new Dictionary<string, object>();

        // chuyen thanh cau query tu store va param truyen vao
        var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, "hs_chatbotai_promt_all", connectionString);

        var resultList = new List<dynamic>();

        // xu ly lay du lieu dua truyen store va param truyen vao

        var result = await _con.Connection_GetSingleDataFromQuery(parameters, "hs_chatbotai_promt_all", connectionString, sqlQuery, sqlParams);

        return $"""
        {(result?.TryGetValue("prompt", out var v) == true
               && !string.IsNullOrWhiteSpace(v?.ToString())
               ? v.ToString()
               : null)}
        """;
      }

      // 2. Lấy danh sách booking + room của khách hàng
      var listBooking = (
          from b in _db.HsBookings
          join r in _db.HsRooms on b.Roomid equals r.Roomid
          where b.Customerid == customer.Customerid
          orderby b.Checkindate descending
          select new
          {
            Booking = b,
            Room = r
          }
      ).ToList();

      // 3. Build nội dung danh sách booking
      var bookingInfoText = new StringBuilder();

      if (!listBooking.Any())
      {
        bookingInfoText.AppendLine("- Khách hàng chưa có đơn đặt phòng nào.");
      }
      else
      {
        int index = 1;
        foreach (var item in listBooking)
        {
          var booking = item.Booking;
          var room = item.Room;

          bookingInfoText.AppendLine($"""
            Đặt phòng #{index}:
            - Phòng: {room.Name}
            - Tầng: {room.Floor}
            - Check-in: {booking.Checkindate:dd/MM/yyyy HH:mm}
            - Check-out: {booking.Checkoutdate:dd/MM/yyyy HH:mm}
            - Trạng thái thanh toán (Đã thanh toán hết hay chưa?): {booking.Ispay}
            - Tổng số tiền: {booking.Totalamount}
            - Cọc: {booking.Deposit}
            - Số tiền còn lại phải thanh toán: {(booking.Ispay == true ? 0 : booking.Totalamount - booking.Deposit)}
            """);

          index++;
        }
      }

      // 4. Build prompt cuối cùng
      return $"""
    Bạn là trợ lý ảo của homestay KOA Home.

    Thông tin khách hàng:
    - Tên khách: {customer.Firstname} {customer.Lastname}
    - Số điện thoại: {customer.Phonenumber}
    - Giới tính: phân biệt từ họ và tên giúp tôi nhé

    Thông tin đặt phòng:
    {bookingInfoText}

    Nội quy:
    - Không hút thuốc
    - Không thú cưng
    - Giữ yên tĩnh sau 22h

    Khách hỏi:
    "{userMessage}"

    Yêu cầu trả lời:
    - Ngắn gọn
    - Thân thiện
    - Tiếng Việt
    - Không nhắc đến AI
    """;
    }
    public async Task<List<ChatHistoryModel>> GetChatHistory()
    {
      string uniqueKey = GetVisitorId();
      string cacheKey = $"History_Gemini_{uniqueKey}";

      var cachedData = await _cache.GetStringAsync(cacheKey);
      List<ChatHistoryModel> history = cachedData != null
          ? JsonConvert.DeserializeObject<List<ChatHistoryModel>>(cachedData)
          : new List<ChatHistoryModel>();

      return history;
    }
    public async Task<string> AskOneShotAsync(string systemPrompt, string userMessage, string selectedModel)
    {
      var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
      var apiKey = env == "Development"
          ? _config["Gemini:ApiKey"]
          : Environment.GetEnvironmentVariable("GEMINI_API_KEY");

      var generateContentConfig = new GoogleGenAIType.GenerateContentConfig
      {
        SystemInstruction = new GoogleGenAIType.Content
        {
          Parts = new List<GoogleGenAIType.Part> { new GoogleGenAIType.Part { Text = systemPrompt } }
        }
      };

      var client = new Client(apiKey: apiKey);
      var response = await client.Models.GenerateContentAsync(
          model: selectedModel,
          contents: new List<GoogleGenAIType.Content>
          {
            new GoogleGenAIType.Content
            {
                Role = "user",
                Parts = new List<GoogleGenAIType.Part> { new GoogleGenAIType.Part { Text = userMessage } }
            }
          },
          config: generateContentConfig
      );

      return response.Candidates[0].Content.Parts[0].Text;
    }
  }
  public class OpenRouterService : IAiService
  {
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private readonly QLKCL_NEWContext _db;
    private readonly OpenAIClient _client;
    private readonly IConnectionService _con;
    private readonly ILogger<IAiService> _logger;
    private readonly IDistributedCache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OpenRouterService(HttpClient http, IConfiguration config, QLKCL_NEWContext db, OpenAIClient client, IConnectionService con, ILogger<IAiService> logger, IDistributedCache cache, IHttpContextAccessor httpContextAccessor)
    {
      _http = http;
      _config = config;
      _db = db;
      _client = client;
      _con = con;
      _logger = logger;
      _cache = cache;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> AskAsync(string message, string prompt, string selectedModel)
    {

      string uniqueKey = GetVisitorId();
      var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
      var apiKey = "";
      if (env == "Development")
      {
        apiKey = _config["OpenRouter:ApiKey"];
      }
      else
      {
        apiKey = Environment.GetEnvironmentVariable("OPENROUTER_API_KEY");
      }

      string cacheKey = $"History_OpenRouter_{uniqueKey}";

      // 1. Lấy lịch sử từ Redis
      var cachedData = await _cache.GetStringAsync(cacheKey);
      List<ChatHistoryModel> history = cachedData != null
          ? JsonConvert.DeserializeObject<List<ChatHistoryModel>>(cachedData)
          : new List<ChatHistoryModel>();

      // Nếu lịch sử chưa từng có prompt của model thì thêm prompt vào đầu tiên để đảm bảo model hiểu ngữ cảnh
      var systemMessage = history.FirstOrDefault(h => h.Role == "system");

      if (systemMessage != null)
      {
        // Nếu đã có, ghi đè nội dung mới (prompt mới cập nhật từ DB/DTO)
        systemMessage.Parts = prompt;
      }
      else
      {
        // Nếu chưa có (lượt chat đầu tiên), thêm vào vị trí đầu tiên
        history.Insert(0, new ChatHistoryModel { Role = "system", Parts = prompt });
      }
      // 2. Thêm tin nhắn mới của User vào lịch sử
      history.Add(new ChatHistoryModel { Role = "user", Parts = message });


      // Giới hạn lịch sử (Ví dụ: Chỉ lấy 10 câu gần nhất để tiết kiệm token)
      if (history.Count > 10)
      {
        // giữ first để giữ prompt của model
        var firstMessage = history.First();
        var lastNineMessages = history.TakeLast(9).ToList();

        history = new List<ChatHistoryModel> { firstMessage };
        history.AddRange(lastNineMessages);
      }


      var baseUrl = _config["OpenRouter:BaseUrl"];
      using var client = new HttpClient();

      // 1. Cấu hình các Header bắt buộc cho OpenRouter
      client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
      client.DefaultRequestHeaders.Add("HTTP-Referer", "https://koahome.vn"); // Bắt buộc để tránh lỗi 401/400
      client.DefaultRequestHeaders.Add("X-Title", "KOA Home Management");

      var modelConfig = new Dictionary<string, double>
      {
          { "deepseek/deepseek-chat", 1.0 },
          { "deepseek/deepseek-r1-0528", 1.0 },
          { "minimax/minimax-m2.5-chat", 0.5 }
      };

      double temperature = modelConfig.ContainsKey(selectedModel)
          ? modelConfig[selectedModel]
          : 0.7;

      // 2. Tạo Body thô nhất có thể để tránh bị "soi" lỗi
      var openRouterHistory = history.Select(h => new
      {
        role = h.Role, // "user" hoặc "model"
        content = h.Parts
      }).ToList();
      var requestBody = new
      {
        model = selectedModel, // Ví dụ: "deepseek/deepseek-chat:free"
        messages = openRouterHistory,
        // Với bản Free, tốt nhất chỉ để lại temperature hoặc bỏ hết
        temperature = temperature
      };

      var response = await client.PostAsJsonAsync($"{baseUrl}/chat/completions", requestBody);

      if (response.IsSuccessStatusCode)
      {
        var json = await response.Content.ReadAsStringAsync();
        // Bạn có thể dùng System.Text.Json để parse lấy content
        using var doc = JsonDocument.Parse(json);
        var botResponse = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();


        // Log query một lần khi trợ lý trả lời
        string singleLineCusContent = message.ToString().Replace(Environment.NewLine, " ").Replace("\n", " ");
        string singleLineBotContent = botResponse.ToString().Replace(Environment.NewLine, " ").Replace("\n", " ");
        _logger.LogInformation($"Khách hỏi: '{singleLineCusContent}' - Bot trả lời '{singleLineBotContent}'");

        // 4. Cập nhật câu trả lời của Bot vào lịch sử
        history.Add(new ChatHistoryModel { Role = "assistant", Parts = botResponse });

        // 5. Lưu lại vào Redis (Set thời gian hết hạn 30 phút - Sliding Expiration)
        var cacheOptions = new DistributedCacheEntryOptions
        {
          SlidingExpiration = TimeSpan.FromMinutes(30)
        };
        await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(history), cacheOptions);

        // 6. Lưu vào DB (Background Task) - Để phân tích nhu cầu khách hàng sau này
        _ = SaveToDatabaseAsync(uniqueKey, message, botResponse);

        return botResponse;
      }
      else
      {
        var errorDetail = await response.Content.ReadAsStringAsync();
        return $"Lỗi API ({response.StatusCode}): {errorDetail}";
      }
    }

    private async Task SaveToDatabaseAsync(string sessionId, string userMsg, string botMsg)
    {
      // Code lưu vào bảng ChatLogs của bạn
      // Nên dùng một Queue hoặc BackgroundService để không làm chậm trải nghiệm khách
    }

    private string GetVisitorId()
    {
      const string CookieName = "KOA_Visitor_Identity";

      // 1. Thử lấy Key từ Cookie của trình duyệt gửi lên
      var visitorId = _httpContextAccessor.HttpContext.Request.Cookies[CookieName];

      if (string.IsNullOrEmpty(visitorId))
      {
        // 2. Nếu chưa có (khách mới), tạo một mã định danh duy nhất (GUID)
        visitorId = Guid.NewGuid().ToString();

        // 3. Cấu hình Cookie để lưu lại trên máy khách
        var cookieOptions = new CookieOptions
        {
          Path = "/",
          HttpOnly = true, // Bảo mật, không cho script đọc
          IsEssential = true, // Hoạt động ngay cả khi khách chưa đồng ý cookie (tùy luật GDPR)
          Expires = DateTimeOffset.UtcNow.AddMinutes(30) // Lưu trong 30 phút, sau đó sẽ hết hạn nếu khách không quay lại
        };

        // 4. Gửi Cookie về trình duyệt
        _httpContextAccessor.HttpContext.Response.Cookies.Append(CookieName, visitorId, cookieOptions);
      }

      return visitorId;
    }

    public string BuildGuestPrompt(int bookingID, string userMessage)
    {
      var customer = new HsCustomer();
      var room = new HsRoom();
      var booking = _db.HsBookings.FirstOrDefault(p => p.Bookingid == bookingID);
      if (booking != null)
      {
        customer = _db.HsCustomers.FirstOrDefault(p => p.Customerid == booking.Customerid);
        room = _db.HsRooms.FirstOrDefault(p => p.Roomid == booking.Roomid);
      }
      return $"""
        Bạn là trợ lý ảo của homestay KOA Home.

        Thông tin đặt phòng:
        - Tên khách: {customer.Firstname} {customer.Lastname}
        - Phòng: {room.Name}
        - Tầng: {room.Floor}
        - Số điện thoại: {customer.Phonenumber}
        - Check-in: {booking.Checkindate:dd/MM/yyyy HH:mm}
        - Check-out: {booking.Checkoutdate:dd/MM/yyyy HH:mm}
        - Trạng thái thanh toán (Đã thanh toán hết hay chưa?): {booking.Ispay}
        - Tổng số tiền: {booking.Totalamount}
        - Cọc: {booking.Deposit}
        - Số tiền còn lại phải thanh toán: {(booking.Ispay == true ? 0 : booking.Totalamount - booking.Deposit)}
        - Nội quy: Không hút thuốc, không thú cưng, giữ yên tĩnh sau 22h

        Khách hỏi:
        "{userMessage}"

        Yêu cầu trả lời:
        - Ngắn gọn
        - Thân thiện
        - Tiếng Việt
        - Không nhắc đến AI
        """;
    }

    public async Task<string> BuildGuestPromptByPhone(string phoneNumber, string userMessage)
    {
      // 1. Lấy thông tin khách hàng theo số điện thoại
      var customer = _db.HsCustomers
          .FirstOrDefault(c => c.Phonenumber == phoneNumber);

      if (customer == null)
      {
        // neu khong truyen connect string thi se lay connection string mac dinh
        string connectionString = _config.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
        var parameters = new Dictionary<string, object>();

        // chuyen thanh cau query tu store va param truyen vao
        var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, "hs_chatbotai_promt_all", connectionString);

        var resultList = new List<dynamic>();

        // xu ly lay du lieu dua truyen store va param truyen vao

        var result = await _con.Connection_GetSingleDataFromQuery(parameters, "hs_chatbotai_promt_all", connectionString, sqlQuery, sqlParams);

        return $"""
        {(result?.TryGetValue("prompt", out var v) == true
               && !string.IsNullOrWhiteSpace(v?.ToString())
               ? v.ToString()
               : null)}
        """;
      }

      // 2. Lấy danh sách booking + room của khách hàng
      var listBooking = (
          from b in _db.HsBookings
          join r in _db.HsRooms on b.Roomid equals r.Roomid
          where b.Customerid == customer.Customerid
          orderby b.Checkindate descending
          select new
          {
            Booking = b,
            Room = r
          }
      ).ToList();

      // 3. Build nội dung danh sách booking
      var bookingInfoText = new StringBuilder();

      if (!listBooking.Any())
      {
        bookingInfoText.AppendLine("- Khách hàng chưa có đơn đặt phòng nào.");
      }
      else
      {
        int index = 1;
        foreach (var item in listBooking)
        {
          var booking = item.Booking;
          var room = item.Room;

          bookingInfoText.AppendLine($"""
            Đặt phòng #{index}:
            - Phòng: {room.Name}
            - Tầng: {room.Floor}
            - Check-in: {booking.Checkindate:dd/MM/yyyy HH:mm}
            - Check-out: {booking.Checkoutdate:dd/MM/yyyy HH:mm}
            - Trạng thái thanh toán (Đã thanh toán hết hay chưa?): {booking.Ispay}
            - Tổng số tiền: {booking.Totalamount}
            - Cọc: {booking.Deposit}
            - Số tiền còn lại phải thanh toán: {(booking.Ispay == true ? 0 : booking.Totalamount - booking.Deposit)}
            """);

          index++;
        }
      }

      // 4. Build prompt cuối cùng
      return $"""
    Bạn là trợ lý ảo của homestay KOA Home.

    Thông tin khách hàng:
    - Tên khách: {customer.Firstname} {customer.Lastname}
    - Số điện thoại: {customer.Phonenumber}
    - Giới tính: phân biệt từ họ và tên giúp tôi nhé

    Thông tin đặt phòng:
    {bookingInfoText}

    Nội quy:
    - Không hút thuốc
    - Không thú cưng
    - Giữ yên tĩnh sau 22h

    Khách hỏi:
    "{userMessage}"

    Yêu cầu trả lời:
    - Ngắn gọn
    - Thân thiện
    - Tiếng Việt
    - Không nhắc đến AI
    """;
    }

    public async Task<List<ChatHistoryModel>> GetChatHistory()
    {

      string uniqueKey = GetVisitorId();
      string cacheKey = $"History_OpenRouter_{uniqueKey}";

      // 1. Lấy lịch sử từ Redis
      var cachedData = await _cache.GetStringAsync(cacheKey);
      List<ChatHistoryModel> history = cachedData != null
          ? JsonConvert.DeserializeObject<List<ChatHistoryModel>>(cachedData)
          : new List<ChatHistoryModel>();

      history = history.Where(h => h.Role == "user" || h.Role == "assistant").ToList();

      return history;
    }

    public async Task<string> AskOneShotAsync(string systemPrompt, string userMessage, string selectedModel)
    {
      var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
      var apiKey = env == "Development"
          ? _config["OpenRouter:ApiKey"]
          : Environment.GetEnvironmentVariable("OPENROUTER_API_KEY");

      var baseUrl = _config["OpenRouter:BaseUrl"];
      using var client = new HttpClient();
      client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
      client.DefaultRequestHeaders.Add("HTTP-Referer", "https://koahome.vn");
      client.DefaultRequestHeaders.Add("X-Title", "KOA Home Management");

      var requestBody = new
      {
        model = selectedModel,
        messages = new object[]
          {
            new { role = "system", content = systemPrompt },
            new { role = "user", content = userMessage }
          },
        temperature = 0.3 // thấp để kết quả phân loại ổn định, ít "sáng tạo"
      };

      var response = await client.PostAsJsonAsync($"{baseUrl}/chat/completions", requestBody);

      if (!response.IsSuccessStatusCode)
      {
        var errorDetail = await response.Content.ReadAsStringAsync();
        throw new Exception($"Lỗi API ({response.StatusCode}): {errorDetail}");
      }

      var json = await response.Content.ReadAsStringAsync();
      using var doc = JsonDocument.Parse(json);
      return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
    }
  }
}
