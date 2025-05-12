using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class WidgetlayoutTest
{
    public int Id { get; set; }

    public string? Widgetid { get; set; }

    public int? Order { get; set; }

    public int? Width { get; set; }

    public int? Height { get; set; }

    public string? Userid { get; set; }

    public string? Dashboardid { get; set; }

    public DateTime? Createdat { get; set; }
}
