using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class Nationality
{
    public int Nationalityid { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdelete { get; set; }

    public string? Nationalitycode { get; set; }

    public string? Nationalityname { get; set; }

    public int? Orderid { get; set; }
}
