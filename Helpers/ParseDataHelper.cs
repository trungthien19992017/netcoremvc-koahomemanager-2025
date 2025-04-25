using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace KOAHome.Helpers
{
  public class ParseDataHelper
  {
    // lay dữ liệu từ options của form field va chuyen ve dang Idictionary
    public static IDictionary<string, object> ParseOptionsToDictionary(string json)
    {
      if (string.IsNullOrWhiteSpace(json)) return new Dictionary<string, object>();

      try
      {
        var jObj = JObject.Parse(json);
        return jObj.Properties()
                   .ToDictionary(p => p.Name, p => (object)p.Value);
      }
      catch (JsonException)
      {
        return new Dictionary<string, object>();
      }
    }
    // lay dữ liệu từ editor options của form field va chuyen ve dang Idictionary
    public static IDictionary<string, object> ParseEditorOptionsToDictionary(string json)
    {
      if (string.IsNullOrWhiteSpace(json)) return new Dictionary<string, object>();

      try
      {
        var jObj = JObject.Parse(json);
        var editorOptions = jObj["editorOptions"] as JObject;

        if (editorOptions == null) return new Dictionary<string, object>();

        return editorOptions.Properties()
                            .ToDictionary(p => p.Name, p => (object)p.Value);
      }
      catch (JsonException)
      {
        return new Dictionary<string, object>();
      }
    }

    // chuyen array json thành List Dictionary
    public static List<Dictionary<string, object>> ParseJsonToListDict(string json)
    {
      return JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
    }

    // lay thong tin validate dua tren List validate truyen vao
    public static Dictionary<string, object>? GetFormFieldValidate(List<Dictionary<string, object>> rules, string type)
    {
      var item = rules.FirstOrDefault(r =>
          r.ContainsKey("type") && r["type"]?.ToString() == type
      );

      return item;
    }

    public static string GetQueryStringFromForm(IFormCollection form, string prefix = "q_")
    {
      var query = form
          .Where(kvp => kvp.Key.StartsWith(prefix))
          .Select(kvp =>
              $"{Uri.EscapeDataString(kvp.Key.Substring(prefix.Length))}={Uri.EscapeDataString(kvp.Value)}"
          );

      return string.Join("&", query);
    }

    public static IFormCollection RemovePrefix_FromFormKey(IFormCollection form, string prefix = "q_")
    {
      var dict = new Dictionary<string, StringValues>();

      foreach (var kvp in form)
      {
        string key = kvp.Key;

        // Nếu key bắt đầu bằng tiền tố q_, thì loại bỏ tiền tố đó
        if (key.StartsWith(prefix))
        {
          key = key.Substring(prefix.Length);
        }

        dict[key] = kvp.Value;
      }

      // Truyền lại form.Files để giữ nguyên file upload
      return new FormCollection(dict, form.Files);
    }

    // chuyển dữ liệu query param dạng "PhoneNumber=&HoTen=&isActive=1&CheckInFrom=2025-04-01&CheckInTo=2025-04-09" thanh Dictionary
    public static Dictionary<string, object> ParseQueryParamsToDictionary(string query)
    {
      var result = new Dictionary<string, object>();

      if (string.IsNullOrWhiteSpace(query))
        return result;

      // Xóa dấu ? nếu có
      if (query.StartsWith("?"))
        query = query.Substring(1);

      var pairs = query.Split('&', StringSplitOptions.RemoveEmptyEntries);

      foreach (var pair in pairs)
      {
        var keyValue = pair.Split('=', 2); // Split thành key=value
        if (keyValue.Length == 2)
        {
          var key = Uri.UnescapeDataString(keyValue[0]);
          var valueStr = Uri.UnescapeDataString(keyValue[1]);

          // Tùy chọn: xử lý giá trị kiểu bool, int, datetime...
          object value = valueStr;

          if (bool.TryParse(valueStr, out var boolVal)) value = boolVal;
          else if (int.TryParse(valueStr, out var intVal)) value = intVal;
          else if (DateTime.TryParse(valueStr, out var dateVal)) value = dateVal;

          result[key] = value;
        }
      }

      return result;
    }

    // thay thế đường dẫn bằng cách replace những tham số bằng result của dòng dữ liệu
    public static string GetReplaceLinkWithResult(string value, IDictionary<string, object>? resultDict, Dictionary<string, object>? paramDict)
    {
      if (string.IsNullOrEmpty(value)) return value;

      // them moi dictionary neu không có dictionary truyền vào
      if (resultDict == null)
      {
        resultDict = new Dictionary<string, object>();
      }
      if (paramDict == null)
      {
        paramDict = new Dictionary<string, object>();
      }

      // gộp giá trị result va param để replace link (ưu tiên result)
      // Nếu muốn key trùng thì result ghi đè param:
      var mergedDict = paramDict
          .Concat(resultDict)
          .GroupBy(kvp => kvp.Key)
          .ToDictionary(g => g.Key, g => g.Last().Value); // dict2 ưu tiên

      // 1. Replace placeholders trong dấu {}
      var replaced = Regex.Replace(value, @"\{(.*?)\}", match =>
      {
        var key = match.Groups[1].Value;
        return mergedDict.TryGetValue(key, out var val) ? val?.ToString() ?? "" : "";
      });

      // 2. Replace các query string dạng key=value
      replaced = Regex.Replace(replaced, @"([?&])(\w+)=([A-Za-z_<>]+)", match =>
      {
        string prefix = match.Groups[1].Value; // ? hoặc &
        string leftKey = match.Groups[2].Value; // key hiển thị
        string rightKey = match.Groups[3].Value; // key tra cứu

        // Nếu là số thì giữ nguyên
        if (int.TryParse(rightKey, out _))
          return match.Value;

        // Nếu bọc trong <>, thì chỉ lấy tên
        if (rightKey.StartsWith("<") && rightKey.EndsWith(">"))
        {
          string stripped = rightKey.Substring(1, rightKey.Length - 2);
          return $"{prefix}{leftKey}={stripped}";
        }

        // Nếu tồn tại trong dict
        if (mergedDict.TryGetValue(rightKey, out var val))
        {
          return $"{prefix}{leftKey}={val}";
        }

        // Không có thì để rỗng
        return $"{prefix}{leftKey}=";
      });

      return replaced;
    }

    public static IDictionary<string, object> GetReplacePopupLinkWithResult(string value, IDictionary<string, object>? resultDict, Dictionary<string, object>? paramDict)
    {
      var result = new Dictionary<string, object>();
      if (string.IsNullOrEmpty(value)) return result;

      var parts = value.Split(',');

      foreach (var part in parts)
      {
        var keyValue = part.Split(':', 2);
        if (keyValue.Length != 2) continue;

        var key = keyValue[0].Trim();
        var val = keyValue[1].Trim();

        if (key.Equals("LINK", StringComparison.OrdinalIgnoreCase))
        {
          val = GetReplaceLinkWithResult(val, resultDict, paramDict);
        }

        result[key] = val;
      }

      return result;
    }

  }
}
