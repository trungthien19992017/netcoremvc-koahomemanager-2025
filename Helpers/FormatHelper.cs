using Microsoft.SqlServer.Server;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Globalization;

namespace KOAHome.Helpers
{
  public static class FormatHelper
  {
    /// <summary>
    /// Format một giá trị động theo định dạng nếu có.
    /// </summary>
    /// <param name="value">Giá trị cần hiển thị</param>
    /// <param name="format">Định dạng nếu có (chuỗi C#, ví dụ: "dd/MM/yyyy", "#,##0.##", ...)</param>
    /// <returns>Chuỗi đã format nếu hợp lệ, hoặc giá trị gốc</returns>
    public static string FormatDynamicValue(object value, string format)
    {
      if (value == null)
        return string.Empty;

      if (string.IsNullOrWhiteSpace(format))
        return value.ToString();

      if (format == "")
        return value.ToString();

      // Nếu là số (decimal / float / double / int)
      if (decimal.TryParse(Convert.ToString(value), out var num))
        return num.ToString(format);

      // Nếu là DateTime
      if (DateTime.TryParse(Convert.ToString(value), out var dt))
        return dt.ToString(format);

      return value.ToString();
    }
    public static string ParseTextAlignToClass(string? textalign)
    {
      if (string.IsNullOrWhiteSpace(textalign))
      {
        textalign = "left";
      }

      if (textalign == "left")
      {
        return "text-start";
      }
      else if (textalign == "center")
      {
        return "text-center";
      }
      else if (textalign == "right")
      {
        return "text-end";
      }
      else
      {
        return "text-truncate";
      }

    }
  }
}
