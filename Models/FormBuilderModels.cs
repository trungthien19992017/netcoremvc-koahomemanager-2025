using System.Text.Json;
using System.Text.Json.Serialization;

namespace KOAHome.Models;

public sealed class FormBuilderSaveRequest
{
  [JsonPropertyName("payload")]
  public JsonElement Payload { get; set; }
}

public sealed class FormBuilderPublishRequest
{
  [JsonPropertyName("formId")]
  public int FormId { get; set; }

  [JsonPropertyName("versionId")]
  public int VersionId { get; set; }

  [JsonPropertyName("expectedLastModificationTime")]
  public DateTimeOffset? ExpectedLastModificationTime { get; set; }
}

public sealed class FormBuilderStoreResult
{
  public bool Success { get; init; }
  public string ErrorMessage { get; init; }
  public string FormCode { get; init; }
  public int? FormId { get; init; }
  public int? VersionId { get; init; }
  public int? Version { get; init; }
  public string Status { get; init; }
  public int SavedFieldCount { get; init; }
  public int SavedServiceCount { get; init; }
  public DateTimeOffset? LastModificationTime { get; init; }
  public JsonElement? ConfigJson { get; init; }
  public JsonElement? CatalogJson { get; init; }
  public JsonElement? Warnings { get; init; }

  public bool IsConcurrencyConflict =>
    !Success && (ErrorMessage?.Contains("concurrency", StringComparison.OrdinalIgnoreCase) == true ||
                 ErrorMessage?.Contains("đã được thay đổi", StringComparison.OrdinalIgnoreCase) == true);
}

public static class FormBuilderPayloadValidator
{
  public const int MaxPayloadBytes = 5 * 1024 * 1024;
  public const int MaxFieldCount = 1000;

  public static string Validate(JsonElement payload)
  {
    if (payload.ValueKind != JsonValueKind.Object)
      return "Payload cấu hình phải là một JSON object.";

    var payloadSize = JsonSerializer.SerializeToUtf8Bytes(payload).Length;
    if (payloadSize > MaxPayloadBytes)
      return $"Payload cấu hình vượt quá {MaxPayloadBytes / 1024 / 1024} MB.";

    if (!payload.TryGetProperty("form", out var form) || form.ValueKind != JsonValueKind.Object)
      return "Thiếu đối tượng form.";

    if (!form.TryGetProperty("code", out var code) || string.IsNullOrWhiteSpace(code.GetString()))
      return "Mã biểu mẫu không được để trống.";

    if (!payload.TryGetProperty("version", out var version) || version.ValueKind != JsonValueKind.Object)
      return "Thiếu đối tượng version đang lưu.";

    if (!version.TryGetProperty("version", out var versionNumber) ||
        versionNumber.ValueKind != JsonValueKind.Number || !versionNumber.TryGetInt32(out var numericVersion) || numericVersion < 1)
      return "Version phải là số nguyên lớn hơn hoặc bằng 1.";

    if (!version.TryGetProperty("fields", out var fields) || fields.ValueKind != JsonValueKind.Array)
      return "Version phải có danh sách fields.";

    if (fields.GetArrayLength() > MaxFieldCount)
      return $"Một version không được vượt quá {MaxFieldCount} trường.";

    return null;
  }
  public sealed class FormBuilderDeleteVersionRequest
  {
    [JsonPropertyName("formId")]
    public int FormId { get; set; }

    [JsonPropertyName("versionId")]
    public int VersionId { get; set; }

    [JsonPropertyName("expectedLastModificationTime")]
    public DateTimeOffset? ExpectedLastModificationTime { get; set; }
  }
}
