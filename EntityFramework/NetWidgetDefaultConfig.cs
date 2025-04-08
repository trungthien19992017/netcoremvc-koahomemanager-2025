using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetWidgetDefaultConfig
{
    public int Id { get; set; }

    public string? Key { get; set; }

    public string? Name { get; set; }

    public string? DefaultValue { get; set; }

    public int WidgetLayoutId { get; set; }

    public bool IsDelete { get; set; }

    public string? Descriptions { get; set; }

    public int? IndexNumber { get; set; }

    public int Index { get; set; }
}
