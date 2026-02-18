using Google.GenAI;
using KOAHome.EntityFramework;
using Npgsql;
using OpenAI;
using System.Text;
using System.Text.Json;

namespace KOAHome.Services
{
  public interface IAiService
  {
    public Task<string> AskAsync(string prompt, string selectedModel);
    public string BuildGuestPrompt(int bookingID, string userMessage);
    public string BuildGuestPromptByPhone(string phoneNumber, string userMessage);
  }
  public class GeminiService : IAiService
  {
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private readonly QLKCL_NEWContext _db;
    private readonly IConnectionService _con;

    public GeminiService(HttpClient http, IConfiguration config, QLKCL_NEWContext db, IConnectionService con)
    {
      _http = http;
      _config = config;
      _db = db;
      _con = con;
    }
    public async Task<string> AskAsync(string prompt, string selectedModel)
    {
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
      var client = new Client(apiKey: apiKey);
      var respone = await client.Models.GenerateContentAsync(
        model: selectedModel,
        contents: prompt
        );
      var answer = respone.Candidates[0].Content.Parts[0].Text;
      return answer;
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

    public string BuildGuestPromptByPhone(string phoneNumber, string userMessage)
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
        var (sqlQuery, sqlParams) = _con.Connection_GetQueryParam(parameters, "hs_homestayai_promt_all", connectionString).Result;

        var resultList = new List<dynamic>();

        // xu ly lay du lieu dua truyen store va param truyen vao

        var result = _con.Connection_GetSingleDataFromQuery(parameters, "hs_homestayai_promt_all", connectionString, sqlQuery, sqlParams);

        return $"""
        Bạn là trợ lý ảo của homestay KOA Home.

        Khách hỏi:
        "{userMessage}"

        Yêu cầu trả lời:
        - Ngắn gọn
        - Thân thiện
        - Tiếng Việt
        - Thông báo không tìm thấy thông tin đặt phòng
        - Không nhắc đến AI

        {(result.Result?.TryGetValue("promt", out var v) == true
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
  }
  public class DeepSeekService : IAiService
  {
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private readonly QLKCL_NEWContext _db;
    private readonly OpenAIClient _client;
    private readonly IConnectionService _con;

    public DeepSeekService(HttpClient http, IConfiguration config, QLKCL_NEWContext db, OpenAIClient client, IConnectionService con)
    {
      _http = http;
      _config = config;
      _db = db;
      _client = client;
      _con = con;
    }
    //public async Task<string> AskAsync(string prompt, string selectedModel)
    //{
    //  var chatClient = _client.GetChatClient(selectedModel);
    //  var messages = new ChatMessage[]
    //  {
    //        new SystemChatMessage(
    //            "Bạn là trợ lý cho hệ thống homestay, trả lời ngắn gọn, thân thiện."
    //        ),
    //        new UserChatMessage(prompt)
    //  };

    //  var response = await chatClient.CompleteChatAsync(messages);

    //  return response.Value.Content[0].Text.Trim();
    //}
    public async Task<string> AskAsync(string prompt, string selectedModel)
    {
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
      var baseUrl = _config["OpenRouter:BaseUrl"];
      using var client = new HttpClient();

      // 1. Cấu hình các Header bắt buộc cho OpenRouter
      client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
      client.DefaultRequestHeaders.Add("HTTP-Referer", "https://koahome.vn"); // Bắt buộc để tránh lỗi 401/400
      client.DefaultRequestHeaders.Add("X-Title", "KOA Home Management");

      // 2. Tạo Body thô nhất có thể để tránh bị "soi" lỗi
      var requestBody = new
      {
        model = selectedModel, // Ví dụ: "deepseek/deepseek-chat:free"
        messages = new[]
          {
            new { role = "user", content = prompt }
        },
        // Với bản Free, tốt nhất chỉ để lại temperature hoặc bỏ hết
        temperature = 1.0
      };

      var response = await client.PostAsJsonAsync($"{baseUrl}/chat/completions", requestBody);

      if (response.IsSuccessStatusCode)
      {
        var json = await response.Content.ReadAsStringAsync();
        // Bạn có thể dùng System.Text.Json để parse lấy content
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
      }
      else
      {
        var errorDetail = await response.Content.ReadAsStringAsync();
        return $"Lỗi API ({response.StatusCode}): {errorDetail}";
      }
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

    public string BuildGuestPromptByPhone(string phoneNumber, string userMessage)
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
        var (sqlQuery, sqlParams) = _con.Connection_GetQueryParam(parameters, "hs_homestayai_promt_all", connectionString).Result;

        var resultList = new List<dynamic>();

        // xu ly lay du lieu dua truyen store va param truyen vao

        var result = _con.Connection_GetSingleDataFromQuery(parameters, "hs_homestayai_promt_all", connectionString, sqlQuery, sqlParams);

        return $"""
        Bạn là trợ lý ảo của homestay KOA Home.

        Khách hỏi:
        "{userMessage}"

        Yêu cầu trả lời:
        - Ngắn gọn
        - Thân thiện
        - Tiếng Việt
        - Thông báo không tìm thấy thông tin đặt phòng
        - Không nhắc đến AI
        
        
        {(result.Result?.TryGetValue("promt", out var v) == true
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
  }
}
