using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HinDistrict
{
    public int? Districtid { get; set; }

    public long? Cityid { get; set; }

    public string? Districtcode { get; set; }

    public string? Districtname { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdelete { get; set; }

    public int? Orderid { get; set; }

    public int? Danso { get; set; }
}
