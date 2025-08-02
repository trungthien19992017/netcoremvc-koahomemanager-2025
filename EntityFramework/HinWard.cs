using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HinWard
{
    public long WardId { get; set; }

    public long? DistrictId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDelete { get; set; }

    public int? OrderId { get; set; }

    public string? WardCode { get; set; }

    public string? WardName { get; set; }
}
