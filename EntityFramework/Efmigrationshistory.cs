using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class Efmigrationshistory
{
    public string Migrationid { get; set; } = null!;

    public string? Productversion { get; set; }
}
