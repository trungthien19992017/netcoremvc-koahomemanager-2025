using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HinCity
{
    public int? Cityid { get; set; }

    public string? Citycode { get; set; }

    public string? Cityname { get; set; }

    public string? Indexid { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdelete { get; set; }
}
