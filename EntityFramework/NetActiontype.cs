using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetActiontype
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public int? Orderid { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdelete { get; set; }

    public int? Siteid { get; set; }

    public string? Sitecode { get; set; }
}
