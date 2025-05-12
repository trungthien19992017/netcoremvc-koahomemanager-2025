using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetWidgetdefaultconfig
{
    public int Id { get; set; }

    public string? Key { get; set; }

    public string? Name { get; set; }

    public string? Defaultvalue { get; set; }

    public int? Widgetlayoutid { get; set; }

    public bool? Isdelete { get; set; }

    public string? Descriptions { get; set; }

    public int? Indexnumber { get; set; }

    public int? Index { get; set; }
}
