using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class Nationality
{
    public long NationalityId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDelete { get; set; }

    public string? NationalityCode { get; set; }

    public string? NationalityName { get; set; }

    public int? OrderId { get; set; }
}
