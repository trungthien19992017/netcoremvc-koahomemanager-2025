using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HinWard
{
    public int? Wardid { get; set; }

    public long? Districtid { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdelete { get; set; }

    public int? Orderid { get; set; }

    public string? Wardcode { get; set; }

    public string? Wardname { get; set; }
}
