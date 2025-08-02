using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class Nation
{
    public long NationId { get; set; }

    public bool IsActive { get; set; }

    public bool IsDelete { get; set; }

    public string NationCode { get; set; } = null!;

    public string NationName { get; set; } = null!;

    public int OrderId { get; set; }
}
