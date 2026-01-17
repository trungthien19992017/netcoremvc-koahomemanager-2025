
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace KOAHome.Services
{
  public interface IGoogleSheetService
  {
    public Task WriteDictionaryToSheet(string spreadsheetId, string sheetName, List<Dictionary<string, object>> data);

    public Task FormatSheet(string spreadsheetId, int sheetId, int totalColumns, int totalRows);

    public Task<int> GetSheetIdByName(string spreadsheetId, string sheetName);

    public Task ApplyRichTextFromHtml(string spreadsheetId, int sheetId, int startRowIndex, int totalRows, int totalColumns);
  }
  public class GoogleSheetService : IGoogleSheetService
  {
    private readonly SheetsService _service;

    public GoogleSheetService(IConfiguration config)
    {
      GoogleCredential credential;

      var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

      if (env == "Development")
      {
        credential = GoogleCredential.FromFile("service-account.json");
      }
      else
      {
        var json = Environment.GetEnvironmentVariable("GOOGLE_SERVICE_ACCOUNT_JSON")
                   .Replace("\\n", "\n");

        credential = GoogleCredential.FromJson(json);
      }

      _service = new SheetsService(new BaseClientService.Initializer
      {
        HttpClientInitializer = credential,
        ApplicationName = "Google Sheet Sync"
      });
    }

    public async Task WriteDictionaryToSheet(
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

    //public async Task ApplyRichTextFromHtml(string spreadsheetId, int sheetId, int startRowIndex, int totalRows, int totalColumns)
    //{
    //  var meta = await _service.Spreadsheets.Get(spreadsheetId).ExecuteAsync();
    //  var sheetMeta = meta.Sheets
    //      .First(s => s.Properties.SheetId == sheetId);

    //  var sheetName = sheetMeta.Properties.Title;

    //  // 2. Load grid data bằng SheetName (A1 notation)
    //  var getRequest = _service.Spreadsheets.Get(spreadsheetId);
    //  getRequest.Ranges = new[] { sheetName };
    //  getRequest.IncludeGridData = true;

    //  var sheet = (await getRequest.ExecuteAsync())
    //      .Sheets.First(s => s.Properties.SheetId == sheetId);

    //  var requests = new List<Request>();
    //  var rows = sheet.Data[0].RowData;

    //  for (int r = startRowIndex; r < rows.Count; r++)
    //  {
    //    var row = rows[r];
    //    if (row.Values == null) continue;

    //    for (int c = 0; c < Math.Min(row.Values.Count, totalColumns); c++)
    //    {
    //      var cell = row.Values[c];
    //      var text = cell.FormattedValue;
    //if (string.IsNullOrEmpty(text) || !text.Contains("{b}"))
    //  continue;

    //var richCell = BuildRichCellFromHtml(text);

    //      requests.Add(new Request
    //      {
    //        UpdateCells = new UpdateCellsRequest
    //        {
    //          Rows = new List<RowData>
    //                {
    //                    new RowData
    //                    {
    //                        Values = new List<CellData> { richCell }
    //                    }
    //                },
    //          Fields = "userEnteredValue,textFormatRuns",
    //          Start = new GridCoordinate
    //          {
    //            SheetId = sheetId,
    //            RowIndex = r,
    //            ColumnIndex = c
    //          }
    //        }
    //      });
    //    }
    //  }

    //  if (!requests.Any()) return;

    //  var batch = new BatchUpdateSpreadsheetRequest
    //  {
    //    Requests = requests
    //  };

    //  await _service.Spreadsheets.BatchUpdate(batch, spreadsheetId).ExecuteAsync();
    //}

    //private CellData BuildRichCellFromHtml(string htmlText)
    //{
    //  var parsed = ParseBoldHtml(htmlText);

    //  var runs = new List<TextFormatRun>
    //  {
    //      new TextFormatRun
    //      {
    //          StartIndex = 0,
    //          Format = new TextFormat { Bold = false }
    //      }
    //  };

    //  foreach (var range in parsed.BoldRanges)
    //  {
    //    runs.Add(new TextFormatRun
    //    {
    //      StartIndex = range.Start,
    //      Format = new TextFormat { Bold = true }
    //    });

    //    runs.Add(new TextFormatRun
    //    {
    //      StartIndex = range.Start + range.Length,
    //      Format = new TextFormat { Bold = false }
    //    });
    //  }

    //  return new CellData
    //  {
    //    UserEnteredValue = new ExtendedValue
    //    {
    //      StringValue = parsed.PlainText
    //    },
    //    TextFormatRuns = runs
    //  };
    //}

    //private RichTextParseResult ParseBoldHtml(string input)
    //{
    //  var result = new RichTextParseResult();

    //  if (string.IsNullOrEmpty(input))
    //  {
    //    result.PlainText = "";
    //    return result;
    //  }

    //  var boldRanges = new List<(int Start, int Length)>();
    //  var plainText = "";
    //  int currentIndex = 0;

    //  var regex = new Regex(@"\{b\}(.*?)\{\/b\}", RegexOptions.IgnoreCase);
    //  int lastIndex = 0;

    //  foreach (Match match in regex.Matches(input))
    //  {
    //    // Text trước {b}
    //    string before = input.Substring(lastIndex, match.Index - lastIndex);
    //    plainText += before;
    //    currentIndex += before.Length;

    //    // Text trong {b}
    //    string boldText = match.Groups[1].Value;

    //    boldRanges.Add((currentIndex, boldText.Length));

    //    plainText += boldText;
    //    currentIndex += boldText.Length;

    //    lastIndex = match.Index + match.Length;
    //  }

    //  // Text còn lại sau cùng
    //  string after = input.Substring(lastIndex);
    //  plainText += after;

    //  result.PlainText = plainText;
    //  result.BoldRanges = boldRanges;

    //  return result;
    //}

    public async Task ApplyRichTextFromHtml(string spreadsheetId, int sheetId, int startRowIndex, int totalRows, int totalColumns)
    {
      var meta = await _service.Spreadsheets.Get(spreadsheetId).ExecuteAsync();
      var sheetMeta = meta.Sheets
          .First(s => s.Properties.SheetId == sheetId);

      var sheetName = sheetMeta.Properties.Title;

      // 2. Load grid data bằng SheetName (A1 notation)
      var getRequest = _service.Spreadsheets.Get(spreadsheetId);
      getRequest.Ranges = new[] { sheetName };
      getRequest.IncludeGridData = true;

      var sheet = (await getRequest.ExecuteAsync())
          .Sheets.First(s => s.Properties.SheetId == sheetId);

      var requests = new List<Request>();
      var rows = sheet.Data[0].RowData;

      for (int r = startRowIndex; r < rows.Count; r++)
      {
        var row = rows[r];
        if (row.Values == null) continue;

        for (int c = 0; c < Math.Min(row.Values.Count, totalColumns); c++)
        {
          var cell = row.Values[c];
          var text = cell.FormattedValue;
          if (string.IsNullOrEmpty(text) || (!text.Contains("{b}") && !text.Contains("{#")))
            continue;

          var richCell = BuildRichCellFromMarkup(text);

          requests.Add(new Request
          {
            UpdateCells = new UpdateCellsRequest
            {
              Rows = new List<RowData>
                    {
                        new RowData
                        {
                            Values = new List<CellData> { richCell }
                        }
                    },
              Fields = "userEnteredValue,textFormatRuns",
              Start = new GridCoordinate
              {
                SheetId = sheetId,
                RowIndex = r,
                ColumnIndex = c
              }
            }
          });
        }
      }

      if (!requests.Any()) return;

      var batch = new BatchUpdateSpreadsheetRequest
      {
        Requests = requests
      };

      await _service.Spreadsheets.BatchUpdate(batch, spreadsheetId).ExecuteAsync();
    }

    private RichTextParseResult ParseRichMarkup(string input)
    {
      var result = new RichTextParseResult
      {
        Ranges = new List<RichFormatRange>()
      };

      if (string.IsNullOrEmpty(input))
      {
        result.PlainText = "";
        return result;
      }

      var stack = new Stack<(bool Bold, Color? Color)>();
      stack.Push((false, null)); // base format

      var plain = new StringBuilder();
      int index = 0;

      var regex = new Regex(@"\{b\}|\{\/b\}|\{#([0-9a-fA-F]{6})\}|\{\/#([0-9a-fA-F]{6})\}",
          RegexOptions.IgnoreCase);

      int last = 0;

      foreach (Match m in regex.Matches(input))
      {
        // text trước tag
        var before = input.Substring(last, m.Index - last);
        AppendText(before);

        var token = m.Value.ToLower();

        if (token == "{b}")
        {
          var current = stack.Peek();
          stack.Push((true, current.Color));
        }
        else if (token == "{/b}")
        {
          stack.Pop();
        }
        else if (token.StartsWith("{#"))
        {
          var color = HexToColor(m.Groups[1].Value);
          var current = stack.Peek();
          stack.Push((current.Bold, color));
        }
        else if (token.StartsWith("{/#"))
        {
          stack.Pop();
        }

        last = m.Index + m.Length;
      }

      AppendText(input.Substring(last));

      result.PlainText = plain.ToString();
      return result;

      void AppendText(string text)
      {
        if (string.IsNullOrEmpty(text)) return;

        var format = stack.Peek();

        if (format.Bold || format.Color != null)
        {
          result.Ranges.Add(new RichFormatRange
          {
            Start = index,
            Length = text.Length,
            Bold = format.Bold,
            Color = format.Color
          });
        }

        plain.Append(text);
        index += text.Length;
      }
    }
    private Color HexToColor(string hex)
    {
      return new Color
      {
        Red = Convert.ToInt32(hex.Substring(0, 2), 16) / 255f,
        Green = Convert.ToInt32(hex.Substring(2, 2), 16) / 255f,
        Blue = Convert.ToInt32(hex.Substring(4, 2), 16) / 255f
      };
    }

    private CellData BuildRichCellFromMarkup(string text)
    {
      var parsed = ParseRichMarkup(text);

      var runs = new List<TextFormatRun>
    {
        new TextFormatRun
        {
            StartIndex = 0,
            Format = new TextFormat()
        }
    };

      foreach (var r in parsed.Ranges)
      {
        var format = new TextFormat
        {
          Bold = r.Bold,
          ForegroundColor = r.Color
        };

        runs.Add(new TextFormatRun
        {
          StartIndex = r.Start,
          Format = format
        });

        runs.Add(new TextFormatRun
        {
          StartIndex = r.Start + r.Length,
          Format = new TextFormat()
        });
      }

      return new CellData
      {
        UserEnteredValue = new ExtendedValue
        {
          StringValue = parsed.PlainText
        },
        TextFormatRuns = runs
      };
    }

    class RichTextParseResult
    {
      public string PlainText { get; set; }
      public List<RichFormatRange> Ranges { get; set; } = new();
    }

    class RichFormatRange
    {
      public int Start { get; set; }
      public int Length { get; set; }
      public bool Bold { get; set; }
      public Color? Color { get; set; }
    }
  }
}
