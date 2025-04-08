using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetWidgetValueConfig
{
    public int Id { get; set; }

    public string? Key { get; set; }

    public string? Name { get; set; }

    public string? Value { get; set; }

    public int WidgetItemId { get; set; }

    public bool IsDelete { get; set; }

    public string? Descriptions { get; set; }

    public int Index { get; set; }
}
