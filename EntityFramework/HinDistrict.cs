using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HinDistrict
{
    public long DistrictId { get; set; }

    public long? CityId { get; set; }

    public string? DistrictCode { get; set; }

    public string? DistrictName { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDelete { get; set; }

    public int? OrderId { get; set; }

    public int? DanSo { get; set; }
}
