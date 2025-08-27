using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class Gender
{
    public long Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }
}
