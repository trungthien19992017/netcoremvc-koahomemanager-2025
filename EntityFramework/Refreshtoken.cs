using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class Refreshtoken
{
    public int Id { get; set; }

    public long? Userid { get; set; }

    public string? Token { get; set; }

    public string? Jwtid { get; set; }

    public bool? Isused { get; set; }

    public bool? Isrevoked { get; set; }

    public DateTime? Issuedat { get; set; }

    public DateTime? Expiredat { get; set; }
}
