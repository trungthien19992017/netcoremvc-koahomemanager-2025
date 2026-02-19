namespace KOAHome.Models
{
  public class ChatHistoryModel
  {
    public string Role { get; set; } // "user" hoặc "model"
    public string Parts { get; set; } // Nội dung tin nhắn
  }
}
