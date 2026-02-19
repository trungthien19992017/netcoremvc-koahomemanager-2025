namespace KOAHome.Models
{
  public class ChatMessage
  {
    public string Role { get; set; } // "user" hoặc "model"
    public string Text { get; set; }
  }
}
