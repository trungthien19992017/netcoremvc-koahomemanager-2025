using KOAHome.Models;
using Npgsql;
using NpgsqlTypes;
using System.Globalization;
using System.Text.Json;

namespace KOAHome.Services;

public interface IFormBuilderService
{
  Task<FormBuilderStoreResult> GetFormBuilderDataAsync(string formCode, int? userId, CancellationToken cancellationToken = default);
  Task<FormBuilderStoreResult> GetFormBuilderCatalogAsync(int? siteId, int? userId, CancellationToken cancellationToken = default);
  Task<FormBuilderStoreResult> SaveFormBuilderAsync(JsonElement payload, int userId, CancellationToken cancellationToken = default);
  Task<FormBuilderStoreResult> PublishFormBuilderAsync(FormBuilderPublishRequest request, int userId, CancellationToken cancellationToken = default);
}

public sealed class FormBuilderService : IFormBuilderService
{
  private readonly string _connectionString;
  private readonly ILogger<FormBuilderService> _logger;

  public FormBuilderService(IConfiguration configuration, ILogger<FormBuilderService> logger)
  {
    _connectionString = configuration.GetConnectionString("ConfigConnection")
      ?? throw new InvalidOperationException("Chưa cấu hình ConnectionStrings:ConfigConnection.");
    _logger = logger;
  }

  public Task<FormBuilderStoreResult> GetFormBuilderDataAsync(string formCode, int? userId, CancellationToken cancellationToken = default)
  {
    var parameters = new[]
    {
      Text("formcode", formCode),
      Integer("userid", userId)
    };
    return ExecuteAsync(
      "CALL dbo.net_form_builder_get(_formcode => @formcode, _userid => @userid);",
      parameters,
      cancellationToken);
  }

  public Task<FormBuilderStoreResult> GetFormBuilderCatalogAsync(int? siteId, int? userId, CancellationToken cancellationToken = default)
  {
    var parameters = new[]
    {
      Integer("siteid", siteId),
      Integer("userid", userId)
    };
    return ExecuteAsync(
      "CALL dbo.net_form_builder_catalog_get(_siteid => @siteid, _userid => @userid);",
      parameters,
      cancellationToken);
  }

  public Task<FormBuilderStoreResult> SaveFormBuilderAsync(JsonElement payload, int userId, CancellationToken cancellationToken = default)
  {
    var parameters = new[]
    {
      new NpgsqlParameter("payload", NpgsqlDbType.Jsonb) { Value = payload.GetRawText() },
      Integer("userid", userId)
    };
    return ExecuteAsync(
      "CALL dbo.net_form_builder_save(_payload => @payload, _userid => @userid);",
      parameters,
      cancellationToken);
  }

  public Task<FormBuilderStoreResult> PublishFormBuilderAsync(FormBuilderPublishRequest request, int userId, CancellationToken cancellationToken = default)
  {
    var expected = new NpgsqlParameter("expectedlastmodificationtime", NpgsqlDbType.TimestampTz)
    {
      Value = request.ExpectedLastModificationTime?.UtcDateTime ?? (object)DBNull.Value
    };
    var parameters = new[]
    {
      Integer("formid", request.FormId),
      Integer("versionid", request.VersionId),
      expected,
      Integer("userid", userId)
    };
    return ExecuteAsync(
      "CALL dbo.net_form_builder_publish(_formid => @formid, _versionid => @versionid, " +
      "_expectedlastmodificationtime => @expectedlastmodificationtime, _userid => @userid);",
      parameters,
      cancellationToken);
  }

  private async Task<FormBuilderStoreResult> ExecuteAsync(
    string callSql,
    IEnumerable<NpgsqlParameter> parameters,
    CancellationToken cancellationToken)
  {
    try
    {
      await using var connection = new NpgsqlConnection(_connectionString);
      await connection.OpenAsync(cancellationToken);

      await using (var call = new NpgsqlCommand(callSql, connection))
      {
        call.CommandTimeout = 120;
        call.Parameters.AddRange(parameters.ToArray());
        await call.ExecuteNonQueryAsync(cancellationToken);
      }

      await using var select = new NpgsqlCommand("SELECT * FROM tmp_result LIMIT 1;", connection)
      {
        CommandTimeout = 120
      };
      await using var reader = await select.ExecuteReaderAsync(cancellationToken);
      if (!await reader.ReadAsync(cancellationToken))
        return Failure("Store không trả dữ liệu trong tmp_result.");

      return new FormBuilderStoreResult
      {
        Success = ReadBoolean(reader, "success"),
        ErrorMessage = ReadString(reader, "errormessage"),
        FormCode = ReadString(reader, "formcode"),
        FormId = ReadInt32(reader, "formid"),
        VersionId = ReadInt32(reader, "versionid"),
        Version = ReadInt32(reader, "version"),
        Status = ReadString(reader, "status"),
        SavedFieldCount = ReadInt32(reader, "savedfieldcount") ?? 0,
        SavedServiceCount = ReadInt32(reader, "savedservicecount") ?? 0,
        LastModificationTime = ReadDateTimeOffset(reader, "lastmodificationtime"),
        ConfigJson = ReadJson(reader, "configjson"),
        CatalogJson = ReadJson(reader, "catalogjson"),
        Warnings = ReadJson(reader, "warnings")
      };
    }
    catch (PostgresException exception)
    {
      _logger.LogError(exception, "Form Builder store failed with SQL state {SqlState}.", exception.SqlState);
      return Failure("Không thể xử lý cấu hình biểu mẫu. Vui lòng kiểm tra store Form Builder và thử lại.");
    }
    catch (OperationCanceledException)
    {
      throw;
    }
    catch (Exception exception)
    {
      _logger.LogError(exception, "Form Builder request failed.");
      return Failure("Không thể kết nối dịch vụ cấu hình biểu mẫu.");
    }
  }

  private static FormBuilderStoreResult Failure(string message) => new() { Success = false, ErrorMessage = message };

  private static NpgsqlParameter Text(string name, string value) =>
    new(name, NpgsqlDbType.Text) { Value = value ?? (object)DBNull.Value };

  private static NpgsqlParameter Integer(string name, int? value) =>
    new(name, NpgsqlDbType.Integer) { Value = value ?? (object)DBNull.Value };

  private static int Ordinal(NpgsqlDataReader reader, string name)
  {
    for (var index = 0; index < reader.FieldCount; index++)
      if (string.Equals(reader.GetName(index), name, StringComparison.OrdinalIgnoreCase))
        return index;
    return -1;
  }

  private static string ReadString(NpgsqlDataReader reader, string name)
  {
    var ordinal = Ordinal(reader, name);
    return ordinal < 0 || reader.IsDBNull(ordinal) ? null : Convert.ToString(reader.GetValue(ordinal), CultureInfo.InvariantCulture);
  }

  private static bool ReadBoolean(NpgsqlDataReader reader, string name)
  {
    var ordinal = Ordinal(reader, name);
    return ordinal >= 0 && !reader.IsDBNull(ordinal) && Convert.ToBoolean(reader.GetValue(ordinal), CultureInfo.InvariantCulture);
  }

  private static int? ReadInt32(NpgsqlDataReader reader, string name)
  {
    var ordinal = Ordinal(reader, name);
    return ordinal < 0 || reader.IsDBNull(ordinal) ? null : Convert.ToInt32(reader.GetValue(ordinal), CultureInfo.InvariantCulture);
  }

  private static DateTimeOffset? ReadDateTimeOffset(NpgsqlDataReader reader, string name)
  {
    var ordinal = Ordinal(reader, name);
    if (ordinal < 0 || reader.IsDBNull(ordinal)) return null;
    var value = reader.GetValue(ordinal);
    return value switch
    {
      DateTimeOffset offset => offset,
      DateTime dateTime => new DateTimeOffset(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)),
      _ => DateTimeOffset.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), out var parsed) ? parsed : null
    };
  }

  private static JsonElement? ReadJson(NpgsqlDataReader reader, string name)
  {
    var ordinal = Ordinal(reader, name);
    if (ordinal < 0 || reader.IsDBNull(ordinal)) return null;
    var value = reader.GetValue(ordinal);
    if (value is JsonElement element) return element.Clone();
    if (value is JsonDocument existingDocument) return existingDocument.RootElement.Clone();
    var json = Convert.ToString(value, CultureInfo.InvariantCulture);
    if (string.IsNullOrWhiteSpace(json)) return null;
    using var document = JsonDocument.Parse(json);
    return document.RootElement.Clone();
  }
}
