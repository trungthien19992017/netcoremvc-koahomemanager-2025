using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HinCity
{
    public long CityId { get; set; }

    public string? CityCode { get; set; }

    public string? CityName { get; set; }

    public string? IndexId { get; set; }

    public bool? IsActive { get; set; }

    public bool IsDelete { get; set; }
}
