using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class WidgetLayoutTest
{
    public int Id { get; set; }

    public string? WidgetId { get; set; }

    public int? Order { get; set; }

    public int? Width { get; set; }

    public int? Height { get; set; }

    public string? UserId { get; set; }

    public string? DashboardId { get; set; }

    public DateTime? CreatedAt { get; set; }
}
