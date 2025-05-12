using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class Radioyesno
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }
}
