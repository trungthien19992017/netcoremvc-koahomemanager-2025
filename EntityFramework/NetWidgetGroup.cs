using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetWidgetGroup
{
    public int Id { get; set; }

    public string? GroupName { get; set; }

    public string? Descriptions { get; set; }

    public bool IsDeleted { get; set; }
}
