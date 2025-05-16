namespace AspnetCoreMvcFull.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    // Thêm exception
    public Exception? Exception { get; set; }
}
