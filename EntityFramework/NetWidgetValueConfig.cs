using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetWidgetvalueconfig
{
    public int Id { get; set; }

    public string? Key { get; set; }

    public string? Name { get; set; }

    public string? Value { get; set; }

    public int? Widgetitemid { get; set; }

    public bool? Isdelete { get; set; }

    public string? Descriptions { get; set; }

    public int? Index { get; set; }
}
