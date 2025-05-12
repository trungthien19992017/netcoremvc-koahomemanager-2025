using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class Nation
{
    public int Nationid { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdelete { get; set; }

    public string? Nationcode { get; set; }

    public string? Nationname { get; set; }

    public int? Orderid { get; set; }
}
