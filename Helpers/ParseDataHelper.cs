using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

      return new FormCollection(dict);
    }

  }
}
