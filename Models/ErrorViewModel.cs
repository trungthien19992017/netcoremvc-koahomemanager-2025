using Microsoft.Data.SqlClient;
using Npgsql;

namespace AspnetCoreMvcFull.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    // Thêm exception
    public SqlException? exception { get; set; }
}
