using System.Globalization;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace KOAHome.Services;

/// <summary>Declarative widget contracts. No database field names or dataset keys belong here.</summary>
public static class DashboardMapper
{
  public static readonly string[] Kinds = ["donut", "line", "bar", "heatmap", "table", "report", "list", "emoji_card", "large_img_card", "multiple_mini_card", "button"];
  public static string Text(JsonNode node, string fallback = "") => node is JsonValue v && v.TryGetValue<string>(out var s) ? s : node?.ToString() ?? fallback;
  public static JsonNode Path(JsonNode root, string path)
  {
    if (string.IsNullOrWhiteSpace(path) || path == "$") return root;
    path = path.StartsWith("$.") ? path[2..] : path.StartsWith('$') ? path[1..] : path;
    if (!Regex.IsMatch(path, @"^(?:[A-Za-z_][A-Za-z0-9_]*|\[\d+\])(?:\.[A-Za-z_][A-Za-z0-9_]*|\[\d+\])*$")) throw new FormatException("Đường dẫn chỉ hỗ trợ tên trường, dấu chấm và chỉ số [0].");
    foreach (Match token in Regex.Matches(path, @"[A-Za-z_][A-Za-z0-9_]*|\[\d+\]"))
    {
      var key = token.Value;
      if (key is "__proto__" or "constructor" or "prototype") throw new FormatException("Tên trường không được hỗ trợ.");
      root = key[0] == '[' ? root is JsonArray a && int.TryParse(key[1..^1], out var n) && n < a.Count ? a[n] : null : root is JsonObject o ? o[key] : null;
      if (root == null) return null;
    }
    return root;
  }
  static JsonNode Read(JsonNode root, JsonNode rule)
  {
    if (rule is not JsonObject obj) return Path(root, Text(rule))?.DeepClone();
    var value = obj.ContainsKey("constant") ? obj["constant"] : Path(root, Text(obj["field"], "$"));
    if (value == null) { if (obj.ContainsKey("default")) return obj["default"]?.DeepClone(); throw new FormatException("Không tìm thấy trường: " + Text(obj["field"])); }
    var split = Text(obj["split"]);
    if (split.Length > 0)
    {
      if (value is not JsonValue) throw new FormatException("Chỉ tách được trường dạng chuỗi.");
      var parts = Text(value).Split(split);
      if (obj["indexPrefix"] != null) return new JsonArray(parts.Select((x,i) => (JsonNode)JsonValue.Create(Text(obj["indexPrefix"]) + (i+1))).ToArray());
      return new JsonArray(parts.Select(x => ConvertValue(JsonValue.Create(x.Trim()), obj)).ToArray());
    }
    if (value is JsonArray array) return new JsonArray(array.Select(x => ConvertValue(x, obj)).ToArray());
    return ConvertValue(value, obj);
  }
  static JsonNode ConvertValue(JsonNode value, JsonObject rule)
  {
    if (value == null) return null;
    var type = Text(rule["type"], "auto");
    if (type == "number")
    {
      var culture = Text(rule["culture"], "invariant");
      if (culture is not ("invariant" or "vi-VN" or "en-US")) throw new FormatException("Định dạng số không hỗ trợ.");
      var text = Text(value).Trim();
      var strip = Text(rule["stripSuffix"]);
      if (strip.Length > 0 && text.EndsWith(strip, StringComparison.Ordinal)) text = text[..^strip.Length].Trim();
      var ci = culture == "invariant" ? CultureInfo.InvariantCulture : CultureInfo.GetCultureInfo(culture);
      var style = culture == "invariant" ? NumberStyles.Float : NumberStyles.Number;
      decimal number;
      if (value is JsonValue numericValue && numericValue.TryGetValue<decimal>(out var nativeNumber)) number = nativeNumber;
      else if (!decimal.TryParse(text, style, ci, out number)) throw new FormatException("Giá trị không phải số theo định dạng đã chọn: " + text[..Math.Min(text.Length, 40)]);
      if (rule["scale"] != null) number *= rule["scale"].GetValue<decimal>();
      if (rule["display"] is JsonObject display) {
        var decimals = Math.Clamp(display["decimals"]?.GetValue<int>() ?? 2, 0, 10);
        var locale = Text(display["locale"], "vi-VN");
        if (locale is not ("vi-VN" or "en-US")) throw new FormatException("Ngôn ngữ định dạng không hỗ trợ.");
        return JsonValue.Create(number.ToString("N"+decimals,CultureInfo.GetCultureInfo(locale))+Text(display["suffix"]));
      }
      return JsonValue.Create(number);
    }
    return type == "string" ? JsonValue.Create(Text(value)) : value.DeepClone();
  }
  static JsonArray Array(JsonNode value, string name) => value as JsonArray ?? throw new FormatException(name + " phải là mảng JSON.");
  static void Numeric(JsonNode value, string name, bool nullable = false)
  {
    if (value == null && nullable) return;
    if (value is not JsonValue v || !v.TryGetValue<decimal>(out _)) throw new FormatException(name + " phải là số JSON; chọn kiểu Số trong ánh xạ.");
  }
  static JsonObject MapObject(JsonNode source, JsonObject mapping)
  {
    var result = new JsonObject();
    foreach (var pair in mapping) {
      if(pair.Key is "__proto__" or "constructor" or "prototype") throw new FormatException("Không dùng tên trường hệ thống làm đích ánh xạ.");
      result[pair.Key] = Read(source, pair.Value);
    }
    return result;
  }
  public static JsonObject Map(string kind, JsonObject binding, JsonNode raw)
  {
    if (!Kinds.Contains(kind)) throw new FormatException("Loại widget không hỗ trợ.");
    var resultConfig = binding["result"] as JsonObject ?? new();
    var root = Path(raw, Text(resultConfig["path"], "$"));
    if (root == null) throw new FormatException("Không tìm thấy đường dẫn gốc của kết quả.");
    if (resultConfig["parseJson"]?.GetValue<bool>() == true) root = JsonNode.Parse(Text(root)) ?? throw new FormatException("Cột kết quả JSON rỗng.");
    var mode = Text(resultConfig["mode"], "direct");
    var mapping = binding["mapping"] as JsonObject ?? new();
    JsonObject data;
    if (mode == "direct") data = root.DeepClone() as JsonObject ?? throw new FormatException("Chế độ Đúng chuẩn cần object JSON; hãy chọn đường dẫn phù hợp.");
    else if (mode == "object") data = MapObject(root, mapping);
    else if (mode == "items")
    {
      data = new JsonObject { ["rows"] = new JsonArray(Array(binding["items"], "items").Select(x => (JsonNode)MapObject(root, x as JsonObject ?? throw new FormatException("Ánh xạ item phải là object."))).ToArray()) };
    }
    else if (mode == "rows")
    {
      var source = Array(root, "Kết quả nguồn");
      IEnumerable<JsonNode> ordered = source;
      var orderBy = Text(resultConfig["orderBy"]);
      if (orderBy.Length > 0) ordered = source.OrderBy(x => Path(x, orderBy), new NodeComparer());
      var rows = ordered.Select(r => MapObject(r, mapping)).ToList();
      if (kind == "donut") data = new JsonObject {
        ["labels"] = new JsonArray(rows.Select(r => r["labels"]?.DeepClone()).ToArray()),
        ["values"] = new JsonArray(rows.Select(r => r["values"]?.DeepClone()).ToArray()),
        ["colors"] = mapping.ContainsKey("colors") ? new JsonArray(rows.Select(r => r["colors"]?.DeepClone()).ToArray()) : new JsonArray()
      };
      else if (kind is "line" or "bar") data = Series(rows, binding);
      else if (kind == "heatmap") data = Heatmap(rows, binding);
      else if (kind is "emoji_card" or "large_img_card" or "button") data = rows.Count == 0 ? new() : rows.Count == 1 ? rows[0] : throw new FormatException("Thẻ đơn cần đúng một dòng; hãy chọn đường dẫn hoặc gom dữ liệu tại nguồn.");
      else data = new JsonObject { ["rows"] = new JsonArray(rows.Select(r => (JsonNode)r).ToArray()) };
    }
    else throw new FormatException("Chế độ ánh xạ không hỗ trợ.");
    if (kind is "line" or "bar" && data["series"] is JsonArray numbers && numbers.Count > 0 && numbers[0] is JsonValue)
      data["series"] = new JsonArray(new JsonObject { ["name"] = Text(binding["seriesLabel"], "Giá trị"), ["data"] = numbers.DeepClone() });
    if (kind is "table" or "report")
    {
      data["columns"] = binding["columns"]?.DeepClone() ?? data["columns"]?.DeepClone() ?? new JsonArray(mapping.Select(p => (JsonNode)new JsonObject { ["field"] = p.Key, ["label"] = p.Key }).ToArray());
    }
    Validate(kind, data);
    return data;
  }
  static JsonObject Series(List<JsonObject> rows, JsonObject binding)
  {
    var labels = rows.Select(r => Text(r["categories"])).Distinct().ToList();
    var groups = rows.Select(r => Text(r["group"], Text(binding["seriesLabel"], "Giá trị"))).Distinct().ToList();
    var series = new JsonArray();
    foreach (var group in groups)
    {
      var values = new JsonArray();
      foreach (var label in labels) values.Add(Cell(rows.Where(r => Text(r["categories"]) == label && Text(r["group"], Text(binding["seriesLabel"], "Giá trị")) == group).Select(r => r["values"]).ToList(), binding));
      series.Add(new JsonObject { ["name"] = group, ["data"] = values });
    }
    return new JsonObject { ["categories"] = new JsonArray(labels.Select(s => (JsonNode)JsonValue.Create(s)).ToArray()), ["series"] = series };
  }
  static JsonObject Heatmap(List<JsonObject> rows, JsonObject binding)
  {
    var xs = rows.Select(r => Text(r["x"])).Distinct().ToList();
    var ys = rows.Select(r => Text(r["y"])).Distinct().ToList();
    if (binding["rowOrder"] is JsonArray order) ys = order.Select(x => Text(x)).Concat(ys).Distinct().ToList();
    var matrix = new JsonObject();
    foreach (var y in ys) matrix[y] = new JsonArray(xs.Select(x => Cell(rows.Where(r => Text(r["x"]) == x && Text(r["y"]) == y).Select(r => r["value"]).ToList(), binding)).ToArray());
    return new JsonObject { ["weekLabels"] = new JsonArray(xs.Select(s => (JsonNode)JsonValue.Create(s)).ToArray()), ["days"] = new JsonArray(ys.Select(s => (JsonNode)JsonValue.Create(s)).ToArray()), ["matrix"] = matrix };
  }
  static JsonNode Cell(List<JsonNode> values, JsonObject binding)
  {
    if (values.Count == 0) return binding["missingValue"]?.DeepClone();
    foreach (var value in values) Numeric(value, "Giá trị ô");
    if (values.Count == 1) return values[0]?.DeepClone();
    var numbers = values.Select(v => v.GetValue<decimal>());
    return Text(binding["aggregate"]) switch {
      "sum" => JsonValue.Create(numbers.Sum()), "avg" => JsonValue.Create(numbers.Average()),
      "min" => JsonValue.Create(numbers.Min()), "max" => JsonValue.Create(numbers.Max()),
      _ => throw new FormatException("Có nhiều dòng cho cùng một ô; chọn cách gom sum/avg/min/max.")
    };
  }
  public static void Validate(string kind, JsonObject data)
  {
    if (kind == "donut")
    {
      var labels = Array(data["labels"], "labels"); var values = Array(data["values"], "values");
      if (labels.Count != values.Count) throw new FormatException("Số nhãn và giá trị không khớp.");
      if (labels.Any(x => x == null || x is not JsonValue)) throw new FormatException("Nhãn Donut phải có giá trị; hãy ánh xạ labels.");
      foreach (var v in values) { Numeric(v, "values"); if (v.GetValue<decimal>() < 0) throw new FormatException("Donut không nhận giá trị âm."); }
      data["colors"] ??= new JsonArray();
      var colors = Array(data["colors"], "colors");
      if (colors.Count != 0 && colors.Count != labels.Count) throw new FormatException("Số màu và nhãn không khớp.");
    }
    else if (kind is "line" or "bar")
    {
      var categories = Array(data["categories"], "categories");
      foreach (var series in Array(data["series"], "series")) {
        if (series is not JsonObject obj || obj["name"] == null) throw new FormatException("Series cần name và data.");
        var values = Array(obj["data"], "series.data"); if (values.Count != categories.Count) throw new FormatException("Số điểm dữ liệu và nhãn trục X không khớp.");
        foreach (var v in values) Numeric(v, "series.data", true);
      }
    }
    else if (kind == "heatmap")
    {
      var xs = Array(data["weekLabels"], "weekLabels");
      foreach (var y in Array(data["days"], "days")) {
        var values = Array(data["matrix"]?[Text(y)], "matrix." + Text(y));
        if (values.Count != xs.Count) throw new FormatException("Kích thước ma trận không khớp nhãn.");
        foreach (var v in values) Numeric(v, "matrix", true);
      }
    }
    else if (kind is "list" or "table" or "report" or "multiple_mini_card") {
      foreach (var row in Array(data["rows"], "rows")) if (row is not JsonObject) throw new FormatException("Mỗi dòng phải là object JSON.");
      if (kind is "table" or "report") foreach(var col in Array(data["columns"], "columns")) if (string.IsNullOrEmpty(Text(col?["field"]))) throw new FormatException("Cột bảng thiếu field.");
    }
  }
  public static bool Empty(string kind, JsonObject data) => kind switch {
    "donut" => data["values"] is JsonArray a && (a.Count == 0 || a.All(x => x?.GetValue<decimal>() == 0)),
    "line" or "bar" => data["categories"] is JsonArray a && a.Count == 0,
    "heatmap" => data["weekLabels"] is JsonArray a && a.Count == 0,
    "list" or "table" or "report" or "multiple_mini_card" => data["rows"] is JsonArray a && a.Count == 0,
    _ => data.Count == 0
  };
  sealed class NodeComparer : IComparer<JsonNode> {
    public int Compare(JsonNode a, JsonNode b) => a is JsonValue av && b is JsonValue bv && av.TryGetValue<decimal>(out var an) && bv.TryGetValue<decimal>(out var bn) ? an.CompareTo(bn) : string.Compare(Text(a), Text(b), StringComparison.Ordinal);
  }
}
