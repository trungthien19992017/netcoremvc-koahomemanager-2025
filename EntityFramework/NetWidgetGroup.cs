using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetWidgetgroup
{
    public int Id { get; set; }

    public string? Groupname { get; set; }

    public string? Descriptions { get; set; }

    public bool? Isdeleted { get; set; }
}
