
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using KOAHome.EntityFramework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace KOAHome.Services
{
  public interface IGoogleSheetService
  {
    void WriteDictionaryToSheet(
        string spreadsheetId,
        string sheetName,
        List<Dictionary<string, object>> data);

    public Task FormatSheet(
      string spreadsheetId,
      int sheetId,
      int totalColumns,
      int totalRows);

    public Task<int> GetSheetIdByName(
        string spreadsheetId,
        string sheetName);

  }
  public class GoogleSheetService : IGoogleSheetService
  {
    private readonly SheetsService _service;

    public GoogleSheetService(IConfiguration config)
    {
      var credentialPath = config["Google:CredentialPath"];
      Console.WriteLine(File.Exists(credentialPath));

      GoogleCredential credential;
      using (var stream = new FileStream(credentialPath, FileMode.Open))
      {
        credential = GoogleCredential.FromStream(stream)
            .CreateScoped(SheetsService.Scope.Spreadsheets);
      }

      _service = new SheetsService(new BaseClientService.Initializer
      {
        HttpClientInitializer = credential,
        ApplicationName = "Google Sheet Sync"
      });
    }

    public void WriteDictionaryToSheet(
        string spreadsheetId,
        string sheetName,
        List<Dictionary<string, object>> data)
    {
      if (data == null || data.Count == 0)
        return;

      var headers = data.First().Keys.ToList();

      var values = new List<IList<object>>
        {
            headers.Cast<object>().ToList()
        };

      foreach (var row in data)
      {
        var line = headers
            .Select(h => ConvertValue(row.ContainsKey(h) ? row[h] : null))
            .ToList();

        values.Add(line);
      }

      var body = new ValueRange { Values = values };

      var request = _service.Spreadsheets.Values.Update(
          body,
          spreadsheetId,
          $"{sheetName}!A1"
      );

      request.ValueInputOption =
          SpreadsheetsResource.ValuesResource.UpdateRequest
              .ValueInputOptionEnum.USERENTERED;

      request.Execute();
    }

    public async Task FormatSheet(
        string spreadsheetId,
        int sheetId,
        int totalColumns,
        int totalRows)
    {
      var requests = new List<Request>();

      // =========================
      // 1. HEADER: in đậm + nền
      // =========================
      requests.Add(new Request
      {
        RepeatCell = new RepeatCellRequest
        {
          Range = new GridRange
          {
            SheetId = sheetId,
            StartRowIndex = 0,
            EndRowIndex = 1
          },
          Cell = new CellData
          {
            UserEnteredFormat = new CellFormat
            {
              TextFormat = new TextFormat
              {
                Bold = true
              },
              BackgroundColor = new Color
              {
                Red = 1.0f,
                Green = 0.95f,
                Blue = 0.6f
              },
              HorizontalAlignment = "CENTER",
              VerticalAlignment = "MIDDLE"
            }
          },
          Fields = "userEnteredFormat(textFormat,backgroundColor,horizontalAlignment,verticalAlignment)"
        }
      });

      // =========================
      // 2. FREEZE HEADER
      // =========================
      requests.Add(new Request
      {
        UpdateSheetProperties = new UpdateSheetPropertiesRequest
        {
          Properties = new SheetProperties
          {
            SheetId = sheetId,
            GridProperties = new GridProperties
            {
              FrozenRowCount = 1
            }
          },
          Fields = "gridProperties.frozenRowCount"
        }
      });

      // =========================
      // 3. BORDER TOÀN BẢNG
      // =========================
      requests.Add(new Request
      {
        UpdateBorders = new UpdateBordersRequest
        {
          Range = new GridRange
          {
            SheetId = sheetId,
            StartRowIndex = 0,
            EndRowIndex = totalRows,
            StartColumnIndex = 0,
            EndColumnIndex = totalColumns
          },
          Top = Border(),
          Bottom = Border(),
          Left = Border(),
          Right = Border(),
          InnerHorizontal = Border(),
          InnerVertical = Border()
        }
      });

      // =========================
      // 4. CONDITIONAL FORMAT
      // (Ví dụ: cột TrạngThái)
      // =========================
      requests.Add(BookingStatusConditionalFormat(
          sheetId,
          statusColumnIndex: 4, // ví dụ cột E
          totalRows: totalRows
      ));

      // =========================
      // 5. AUTO SCALE COLUMNS (LUÔN ĐẶT CUỐI)
      // =========================
      requests.Add(new Request
      {
        AutoResizeDimensions = new AutoResizeDimensionsRequest
        {
          Dimensions = new DimensionRange
          {
            SheetId = sheetId,
            Dimension = "COLUMNS",
            StartIndex = 0,
            EndIndex = totalColumns
          }
        }
      });

      // =========================
      // EXECUTE
      // =========================
      var batchRequest = new BatchUpdateSpreadsheetRequest
      {
        Requests = requests
      };

      await _service.Spreadsheets
          .BatchUpdate(batchRequest, spreadsheetId)
          .ExecuteAsync();
    }

    private object ConvertValue(object value)
    {
      if (value == null) return "";

      // 1. Xử lý string
      if (value is string s)
      {
        if (string.IsNullOrWhiteSpace(s))
          return "";

        // Nếu có HTML → strip trước
        if (s.Contains("<"))
          s = StripHtml(s);

        // Chuẩn hóa xuống dòng cho Google Sheet
        s = NormalizeNewLine(s);

        return s;
      }

      return value switch
      {
        DateTime dt => dt.ToString("yyyy-MM-dd HH:mm:ss"),
        _ => value
      };
    }
    private static string NormalizeNewLine(string input)
    {
      if (string.IsNullOrEmpty(input))
        return "";

      return input
          .Replace("\r\n", "\n")   // Windows
          .Replace("\r", "\n")     // Old Mac
          .Replace("\\n", "\n")    // Text literal từ JSON / DB
          .Trim();
    }
    private string StripHtml(string input)
    {
      if (string.IsNullOrEmpty(input))
        return "";

      return Regex.Replace(input, "<.*?>", string.Empty)
                  .Replace("&nbsp;", " ")
                  .Trim();
    }
    private static Border Border()
    {
      return new Border
      {
        Style = "SOLID",
        Width = 1,
        Color = new Color { Red = 0.8f, Green = 0.8f, Blue = 0.8f }
      };
    }
    private Request BookingStatusConditionalFormat(
    int sheetId,
    int statusColumnIndex,
    int totalRows)
    {
      return new Request
      {
        AddConditionalFormatRule = new AddConditionalFormatRuleRequest
        {
          Rule = new ConditionalFormatRule
          {
            Ranges = new List<GridRange>
                {
                    new GridRange
                    {
                        SheetId = sheetId,
                        StartRowIndex = 1,
                        EndRowIndex = totalRows,
                        StartColumnIndex = statusColumnIndex,
                        EndColumnIndex = statusColumnIndex + 1
                    }
                },
            BooleanRule = new BooleanRule
            {
              Condition = new BooleanCondition
              {
                Type = "TEXT_CONTAINS",
                Values = new List<ConditionValue>
                        {
                            new ConditionValue { UserEnteredValue = "Có khách" }
                        }
              },
              Format = new CellFormat
              {
                BackgroundColor = new Color
                {
                  Red = 1f,
                  Green = 0.8f,
                  Blue = 0.8f
                }
              }
            }
          },
          Index = 0
        }
      };
    }

    public async Task<int> GetSheetIdByName(
        string spreadsheetId,
        string sheetName)
    {
      var spreadsheet = await _service.Spreadsheets
          .Get(spreadsheetId)
          .ExecuteAsync();

      var sheet = spreadsheet.Sheets
          .FirstOrDefault(s => s.Properties.Title == sheetName);

      if (sheet == null)
        throw new Exception($"Không tìm thấy sheet: {sheetName}");

      return sheet.Properties.SheetId.Value;
    }

  }
}
