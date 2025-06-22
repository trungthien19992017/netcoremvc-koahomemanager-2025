namespace KOAHome.Models
{
  public class CloudflareR2Config
  {
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string Bucket { get; set; }
    public string AccountId { get; set; }
    public string Region { get; set; } = "auto";
  }
}
